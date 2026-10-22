namespace RemoteDesktopLog.Core.Models;

/// <summary>Servers 残留清理页一行：MRU 中已不存在、但 Servers 下仍残留的子键。</summary>
public sealed class OrphanServerInfo
{
    /// <summary>Servers 子键名（即服务器主机名）。</summary>
    public string KeyName { get; set; } = "";

    public string? UsernameHint { get; set; }

    public bool HasCertHash { get; set; }

    public bool HasStoredCredential { get; set; }
}
