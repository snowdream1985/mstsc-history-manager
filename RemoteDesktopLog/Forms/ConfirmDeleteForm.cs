namespace RemoteDesktopLog.Forms;

/// <summary>删除确认对话框：列出待删服务器，"同时清除附加信息"复选框默认勾选、可取消。</summary>
public sealed class ConfirmDeleteForm : Form
{
    private readonly CheckBox _chkClear;

    public bool ClearAssociatedData => _chkClear.Checked;

    public ConfirmDeleteForm(IReadOnlyList<string> serverNames)
    {
        Text = "确认删除";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowIcon = false;
        ClientSize = new Size(440, 280);

        var lblMessage = new Label
        {
            Text = $"确定要删除选中的 {serverNames.Count} 条记录吗？",
            AutoSize = true,
            Location = new Point(16, 16),
        };

        var lstServers = new ListBox
        {
            Location = new Point(16, 44),
            Size = new Size(408, 128),
            IntegralHeight = false,
        };
        const int maxShown = 8;
        foreach (var s in serverNames.Take(maxShown))
            lstServers.Items.Add(s);
        if (serverNames.Count > maxShown)
            lstServers.Items.Add($"…等共 {serverNames.Count} 条");

        _chkClear = new CheckBox
        {
            Text = "同时清除这些服务器的用户名提示、证书指纹和已保存的凭据",
            Checked = true,
            AutoSize = true,
            Location = new Point(16, 182),
        };

        var btnOk = new Button
        {
            Text = "删除",
            DialogResult = DialogResult.OK,
            Location = new Point(248, 226),
            Size = new Size(85, 30),
        };
        var btnCancel = new Button
        {
            Text = "取消",
            DialogResult = DialogResult.Cancel,
            Location = new Point(339, 226),
            Size = new Size(85, 30),
        };

        Controls.Add(lblMessage);
        Controls.Add(lstServers);
        Controls.Add(_chkClear);
        Controls.Add(btnOk);
        Controls.Add(btnCancel);
        AcceptButton = btnOk;
        CancelButton = btnCancel;
    }
}
