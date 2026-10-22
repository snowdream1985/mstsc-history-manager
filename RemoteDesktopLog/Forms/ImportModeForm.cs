namespace RemoteDesktopLog.Forms;

public enum ImportMode
{
    None,
    /// <summary>清空当前列表，使用备份内容。</summary>
    Replace,
    /// <summary>保留当前列表，备份中的新记录追加到末尾。</summary>
    Merge,
}

/// <summary>导入方式选择：替换 / 合并 / 取消。</summary>
public sealed class ImportModeForm : Form
{
    public ImportMode Mode { get; private set; } = ImportMode.None;

    public ImportModeForm(int importedCount)
    {
        Text = "导入方式";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowIcon = false;
        ClientSize = new Size(430, 178);

        var lbl = new Label
        {
            Text = $"备份文件中包含 {importedCount} 条记录。请选择导入方式：\r\n\r\n" +
                   "替换：清空当前列表，使用备份内容。\r\n" +
                   "合并：保留当前列表，备份中的新记录追加到末尾。",
            Location = new Point(16, 14),
            Size = new Size(400, 84),
        };

        var btnReplace = new Button
        {
            Text = "替换当前列表",
            Location = new Point(16, 112),
            Size = new Size(125, 32),
        };
        var btnMerge = new Button
        {
            Text = "合并到当前列表",
            Location = new Point(153, 112),
            Size = new Size(125, 32),
        };
        var btnCancel = new Button
        {
            Text = "取消",
            DialogResult = DialogResult.Cancel,
            Location = new Point(329, 112),
            Size = new Size(85, 32),
        };

        btnReplace.Click += (_, _) => { Mode = ImportMode.Replace; DialogResult = DialogResult.OK; };
        btnMerge.Click += (_, _) => { Mode = ImportMode.Merge; DialogResult = DialogResult.OK; };

        Controls.Add(lbl);
        Controls.Add(btnReplace);
        Controls.Add(btnMerge);
        Controls.Add(btnCancel);
        CancelButton = btnCancel;
    }
}
