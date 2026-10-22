namespace RemoteDesktopLog.Forms;

/// <summary>手动添加服务器到下拉列表（插入顶部，已存在则提升）。</summary>
public sealed class AddServerForm : Form
{
    private readonly TextBox _txtServer;
    private readonly Button _btnOk;

    public string ServerName => _txtServer.Text.Trim();

    public AddServerForm()
    {
        Text = "添加服务器";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowIcon = false;
        ClientSize = new Size(440, 132);

        var lbl = new Label
        {
            Text = "服务器地址（可带端口，如 192.168.1.1 或 192.168.1.1:3389）：",
            AutoSize = true,
            Location = new Point(16, 16),
        };

        _txtServer = new TextBox
        {
            Location = new Point(16, 42),
            Size = new Size(408, 23),
        };

        _btnOk = new Button
        {
            Text = "添加",
            DialogResult = DialogResult.OK,
            Enabled = false,
            Location = new Point(248, 82),
            Size = new Size(85, 30),
        };
        var btnCancel = new Button
        {
            Text = "取消",
            DialogResult = DialogResult.Cancel,
            Location = new Point(339, 82),
            Size = new Size(85, 30),
        };

        _txtServer.TextChanged += (_, _) => _btnOk.Enabled = !string.IsNullOrWhiteSpace(_txtServer.Text);

        Controls.Add(lbl);
        Controls.Add(_txtServer);
        Controls.Add(_btnOk);
        Controls.Add(btnCancel);
        AcceptButton = _btnOk;
        CancelButton = btnCancel;
    }
}
