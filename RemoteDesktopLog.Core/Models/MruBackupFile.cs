namespace RemoteDesktopLog.Core.Models;

/// <summary>备份 JSON 文件格式。</summary>
public sealed class MruBackupFile
{
    public int Version { get; set; } = 1;

    public DateTimeOffset ExportedAt { get; set; }

    /// <summary>按 MRU 顺序（顶部在前）的服务器列表。</summary>
    public List<string> Servers { get; set; } = new();
}
