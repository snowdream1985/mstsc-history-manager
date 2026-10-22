using System.ComponentModel;
using System.Diagnostics;
using RemoteDesktopLog.Core.Models;
using RemoteDesktopLog.Core.Services;
using RemoteDesktopLog.Forms;

namespace RemoteDesktopLog;

public partial class MainForm : Form
{
    private readonly RdpRegistryService _registry = new();
    private readonly CredentialService _credentials = new();
    private readonly RdpBackupService _backup = new();

    /// <summary>当前 MRU 列表（与注册表同步的最新读取结果）。</summary>
    private List<string> _mruList = new();

    public MainForm()
    {
        InitializeComponent();
        WireEvents();
    }

    private void WireEvents()
    {
        tsbRefresh.Click += (_, _) => RefreshAll();
        tsbAdd.Click += (_, _) => AddServer();
        tsbDelete.Click += (_, _) => DeleteSelected();
        tsbBatchDelete.Click += (_, _) => BatchDelete();
        tsbMoveUp.Click += (_, _) => MoveSelected(-1);
        tsbMoveDown.Click += (_, _) => MoveSelected(+1);
        tsbExport.Click += (_, _) => ExportBackup();
        tsbImport.Click += (_, _) => ImportBackup();
        tsbConnect.Click += (_, _) => ConnectSelected();

        mnuConnect.Click += (_, _) => ConnectSelected();
        mnuMoveUp.Click += (_, _) => MoveSelected(-1);
        mnuMoveDown.Click += (_, _) => MoveSelected(+1);
        mnuDelete.Click += (_, _) => DeleteSelected();
        mnuCopy.Click += (_, _) => CopySelected();
        mnuGrid.Opening += (_, _) => UpdateButtonStates();

        gridMru.SelectionChanged += (_, _) => UpdateButtonStates();
        gridMru.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) ConnectSelected(); };
        gridMru.CellFormatting += BoolColumnFormatting;
        gridMru.KeyDown += GridMruKeyDown;

        gridOrphan.CellFormatting += BoolColumnFormatting;
        btnOrphanDeleteSelected.Click += (_, _) => DeleteOrphans(SelectedOrphans(), confirm: false);
        btnOrphanDeleteAll.Click += (_, _) => DeleteOrphans(AllOrphans(), confirm: true);

        mstscTimer.Tick += (_, _) => UpdateMstscStatus();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        RefreshAll();
        UpdateButtonStates();
        UpdateMstscStatus();
        mstscTimer.Start();
    }

    // ---------- 读取与刷新 ----------

    private void RefreshAll()
    {
        try
        {
            _mruList = _registry.ReadMruList().ToList();
            var creds = _credentials.EnumerateTermsrvTargets();

            var rows = _mruList.Select((s, i) =>
            {
                var host = ServerNameParser.ExtractHost(s);
                var meta = _registry.ReadServerMeta(host);
                return new RdpServerEntry
                {
                    Order = i + 1,
                    ServerName = s,
                    UsernameHint = meta.UsernameHint,
                    HasCertHash = meta.HasCertHash,
                    HasServersKey = _registry.ServerKeyExists(host),
                    HasStoredCredential = creds.ContainsKey(host),
                };
            }).ToList();
            gridMru.DataSource = new BindingSource { DataSource = new BindingList<RdpServerEntry>(rows) };

            var mruHosts = new HashSet<string>(_mruList.Select(ServerNameParser.ExtractHost), StringComparer.OrdinalIgnoreCase);
            var orphans = _registry.ReadAllServerKeys()
                .Where(o => !mruHosts.Contains(o.KeyName))
                .Select(o => { o.HasStoredCredential = creds.ContainsKey(o.KeyName); return o; })
                .ToList();
            gridOrphan.DataSource = new BindingSource { DataSource = new BindingList<OrphanServerInfo>(orphans) };

            lblCount.Text = $"共 {_mruList.Count} 条记录，残留 {orphans.Count} 条";
            if (!_registry.IsMruNumberingContiguous())
                lblCount.Text += "（编号不连续，写操作将自动重排）";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "读取失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ---------- 写路径 ----------

    /// <summary>写注册表前的 mstsc 防护：运行时弹重试/取消循环。</summary>
    private bool EnsureMstscClosed()
    {
        while (MstscProcessGuard.IsMstscRunning())
        {
            var r = MessageBox.Show(this,
                "检测到远程桌面连接 (mstsc.exe) 正在运行。\r\n它退出时会用内存中的旧历史记录覆盖您的修改。\r\n\r\n" +
                "请先关闭所有远程桌面窗口，然后点击\"重试\"。",
                "请先关闭远程桌面", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
            if (r == DialogResult.Cancel)
                return false;
        }
        return true;
    }

    /// <summary>所有 MRU 写操作的统一入口：防护 → 落盘 → 可选关联清理 → 校验 → 刷新。</summary>
    private bool ApplyNewMruList(List<string> newList, IReadOnlyList<string>? cleanupHosts = null)
    {
        if (!EnsureMstscClosed())
            return false;

        try
        {
            _registry.WriteMruList(newList);
        }
        catch (MstscRunningException ex)
        {
            MessageBox.Show(this, ex.Message, "请先关闭远程桌面", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "写入注册表失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        if (cleanupHosts is { Count: > 0 })
        {
            var failures = new List<string>();
            foreach (var host in cleanupHosts.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (!_registry.TryDeleteServerKey(host, out var err))
                    failures.Add($"{host}（Servers 子键：{err}）");
                if (!_credentials.DeleteCredential(host))
                    failures.Add($"{host}（凭据删除失败）");
            }
            if (failures.Count > 0)
                MessageBox.Show(this, "部分附加信息清除失败：\r\n" + string.Join("\r\n", failures),
                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // 写后校验：mstsc 竞态启动可能导致写入被覆盖
        if (!_registry.ReadMruList().SequenceEqual(newList))
            MessageBox.Show(this, "写入后校验不一致，修改可能被正在运行的 mstsc 覆盖，请刷新确认。",
                "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        RefreshAll();
        return true;
    }

    // ---------- 各操作 ----------

    private void DeleteSelected()
    {
        var rows = SelectedMruRows();
        if (rows.Count == 0)
            return;

        var names = rows.Select(r => r.ServerName).ToList();
        using var dlg = new ConfirmDeleteForm(names);
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        var newList = _mruList.ToList();
        MruListEditor.RemoveAt(newList, rows.Select(r => r.Order - 1).ToList());
        var cleanup = dlg.ClearAssociatedData
            ? names.Select(ServerNameParser.ExtractHost).ToList()
            : null;
        ApplyNewMruList(newList, cleanup);
    }

    private void BatchDelete()
    {
        using var dlg = new BatchDeleteForm(_mruList);
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        var newList = _mruList.ToList();
        MruListEditor.RemoveAt(newList, dlg.MatchIndexes);
        ApplyNewMruList(newList);
    }

    private void AddServer()
    {
        using var dlg = new AddServerForm();
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        var newList = _mruList.ToList();
        MruListEditor.AddToTop(newList, dlg.ServerName);
        if (ApplyNewMruList(newList))
            SelectMruRow(0);
    }

    private void MoveSelected(int delta)
    {
        if (gridMru.SelectedRows.Count != 1)
            return;
        var entry = (RdpServerEntry)gridMru.SelectedRows[0].DataBoundItem;
        var index = entry.Order - 1;

        var newList = _mruList.ToList();
        if (!MruListEditor.Move(newList, index, delta))
            return;
        if (ApplyNewMruList(newList))
            SelectMruRow(index + delta);
    }

    private void ExportBackup()
    {
        if (_mruList.Count == 0)
        {
            MessageBox.Show(this, "当前没有记录可导出。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dlg = new SaveFileDialog
        {
            Filter = "JSON 文件|*.json",
            FileName = $"rdp-mru-{DateTime.Now:yyyyMMdd-HHmmss}.json",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            _backup.Export(dlg.FileName, _mruList);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "导出失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ImportBackup()
    {
        using var ofd = new OpenFileDialog { Filter = "JSON 文件|*.json" };
        if (ofd.ShowDialog(this) != DialogResult.OK)
            return;

        IReadOnlyList<string> imported;
        try
        {
            imported = _backup.Import(ofd.FileName);
        }
        catch (FormatException ex)
        {
            MessageBox.Show(this, ex.Message, "导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        using var modeDlg = new ImportModeForm(imported.Count);
        modeDlg.ShowDialog(this);
        List<string> newList = modeDlg.Mode switch
        {
            ImportMode.Replace => imported.ToList(),
            ImportMode.Merge => MruListEditor.Merge(_mruList, imported),
            _ => _mruList,
        };
        if (modeDlg.Mode == ImportMode.None)
            return;
        ApplyNewMruList(newList);
    }

    private void ConnectSelected()
    {
        if (gridMru.SelectedRows.Count != 1)
            return;
        var entry = (RdpServerEntry)gridMru.SelectedRows[0].DataBoundItem;
        try
        {
            Process.Start(new ProcessStartInfo("mstsc.exe", $"/v:{entry.ServerName}") { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "启动 mstsc 失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DeleteOrphans(IReadOnlyList<OrphanServerInfo> targets, bool confirm)
    {
        if (targets.Count == 0)
        {
            MessageBox.Show(this, "没有需要清除的残留记录。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (confirm && MessageBox.Show(this,
                $"确定要清除全部 {targets.Count} 条残留记录吗？",
                "确认清除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        if (!EnsureMstscClosed())
            return;

        var deleteCred = chkOrphanDeleteCred.Checked;
        var failures = new List<string>();
        foreach (var o in targets)
        {
            if (!_registry.TryDeleteServerKey(o.KeyName, out var err))
                failures.Add($"{o.KeyName}（{err}）");
            else if (deleteCred && o.HasStoredCredential && !_credentials.DeleteCredential(o.KeyName))
                failures.Add($"{o.KeyName}（凭据删除失败）");
        }
        if (failures.Count > 0)
            MessageBox.Show(this, "部分清除失败：\r\n" + string.Join("\r\n", failures),
                "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        RefreshAll();
    }

    // ---------- 辅助 ----------

    private List<RdpServerEntry> SelectedMruRows() =>
        gridMru.SelectedRows.Cast<DataGridViewRow>()
            .Select(r => (RdpServerEntry)r.DataBoundItem)
            .OrderBy(e => e.Order)
            .ToList();

    private List<OrphanServerInfo> SelectedOrphans() =>
        gridOrphan.SelectedRows.Cast<DataGridViewRow>()
            .Select(r => (OrphanServerInfo)r.DataBoundItem)
            .ToList();

    private List<OrphanServerInfo> AllOrphans() =>
        gridOrphan.Rows.Cast<DataGridViewRow>()
            .Select(r => (OrphanServerInfo)r.DataBoundItem)
            .ToList();

    private void SelectMruRow(int index)
    {
        if (index < 0 || index >= gridMru.Rows.Count)
            return;
        gridMru.ClearSelection();
        gridMru.Rows[index].Selected = true;
        gridMru.CurrentCell = gridMru.Rows[index].Cells[0];
    }

    private void CopySelected()
    {
        var names = SelectedMruRows().Select(r => r.ServerName).ToList();
        if (names.Count > 0)
            Clipboard.SetText(string.Join(Environment.NewLine, names));
    }

    private void GridMruKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete)
        {
            DeleteSelected();
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.C)
        {
            CopySelected();
            e.Handled = true;
        }
    }

    private void UpdateButtonStates()
    {
        var single = gridMru.SelectedRows.Count == 1;
        var index = single ? ((RdpServerEntry)gridMru.SelectedRows[0].DataBoundItem).Order - 1 : -1;
        var any = gridMru.SelectedRows.Count > 0;

        tsbConnect.Enabled = mnuConnect.Enabled = single;
        tsbMoveUp.Enabled = mnuMoveUp.Enabled = single && index > 0;
        tsbMoveDown.Enabled = mnuMoveDown.Enabled = single && index >= 0 && index < gridMru.Rows.Count - 1;
        tsbDelete.Enabled = mnuDelete.Enabled = any;
    }

    private void UpdateMstscStatus()
    {
        var running = MstscProcessGuard.IsMstscRunning();
        lblMstsc.Text = running ? "⚠ 检测到 mstsc 正在运行，写操作前请关闭" : "mstsc 未运行";
        lblMstsc.ForeColor = running ? Color.Red : SystemColors.ControlText;
    }

    /// <summary>布尔列显示为"有"/"—"。</summary>
    private void BoolColumnFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (sender is not DataGridView grid || e.ColumnIndex < 0)
            return;
        var prop = grid.Columns[e.ColumnIndex].DataPropertyName;
        if (prop is "HasCertHash" or "HasStoredCredential" && e.Value is bool b)
        {
            e.Value = b ? "有" : "—";
            e.FormattingApplied = true;
        }
    }
}
