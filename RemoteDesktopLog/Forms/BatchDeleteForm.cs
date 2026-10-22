using RemoteDesktopLog.Core.Services;

namespace RemoteDesktopLog.Forms;

/// <summary>通配符批量删除：实时预览匹配数量与明细，匹配 0 条时禁用确定。</summary>
public sealed class BatchDeleteForm : Form
{
    private readonly IReadOnlyList<string> _mruList;
    private readonly TextBox _txtPattern;
    private readonly Label _lblCount;
    private readonly ListBox _lstPreview;
    private readonly Button _btnOk;

    /// <summary>当前通配符在 MRU 列表中匹配到的索引。</summary>
    public IReadOnlyList<int> MatchIndexes { get; private set; } = Array.Empty<int>();

    public BatchDeleteForm(IReadOnlyList<string> mruList)
    {
        _mruList = mruList;

        Text = "批量删除（通配符）";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowIcon = false;
        ClientSize = new Size(440, 336);

        var lbl = new Label
        {
            Text = "通配符（* 匹配任意长度，? 匹配单字符）：",
            AutoSize = true,
            Location = new Point(16, 16),
        };

        _txtPattern = new TextBox
        {
            Location = new Point(16, 42),
            Size = new Size(408, 23),
            PlaceholderText = "例: 127.0.0.1:*",
        };

        _lblCount = new Label
        {
            Text = "匹配 0 条记录",
            AutoSize = true,
            Location = new Point(16, 74),
        };

        _lstPreview = new ListBox
        {
            Location = new Point(16, 96),
            Size = new Size(408, 162),
            IntegralHeight = false,
        };

        _btnOk = new Button
        {
            Text = "删除",
            DialogResult = DialogResult.OK,
            Enabled = false,
            Location = new Point(248, 274),
            Size = new Size(85, 30),
        };
        var btnCancel = new Button
        {
            Text = "取消",
            DialogResult = DialogResult.Cancel,
            Location = new Point(339, 274),
            Size = new Size(85, 30),
        };

        _txtPattern.TextChanged += (_, _) => UpdatePreview();

        Controls.Add(lbl);
        Controls.Add(_txtPattern);
        Controls.Add(_lblCount);
        Controls.Add(_lstPreview);
        Controls.Add(_btnOk);
        Controls.Add(btnCancel);
        AcceptButton = _btnOk;
        CancelButton = btnCancel;
    }

    private void UpdatePreview()
    {
        MatchIndexes = WildcardMatcher.MatchIndexes(_mruList, _txtPattern.Text.Trim());
        _lstPreview.Items.Clear();
        foreach (var i in MatchIndexes)
            _lstPreview.Items.Add(_mruList[i]);
        _lblCount.Text = $"匹配 {MatchIndexes.Count} 条记录";
        _btnOk.Enabled = MatchIndexes.Count > 0;
    }
}
