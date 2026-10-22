namespace RemoteDesktopLog.Core.Models;

/// <summary>主表格一行：一条 MRU 记录及其关联信息。</summary>
public sealed class RdpServerEntry
{
    /// <summary>显示序号（1 起，等于 MRU 编号 + 1）。</summary>
    public int Order { get; set; }

    /// <summary>MRU 原始字符串（可能带端口或 IPv6）。</summary>
    public string ServerName { get; set; } = "";

    /// <summary>Servers 子键中的 UsernameHint，无则为 null。</summary>
    public string? UsernameHint { get; set; }

    /// <summary>Servers 子键中是否存在非空 CertHash。</summary>
    public bool HasCertHash { get; set; }

    /// <summary>凭据管理器中是否存在 TERMSRV/&lt;host&gt; 凭据。</summary>
    public bool HasStoredCredential { get; set; }

    /// <summary>Servers 下是否存在对应子键。</summary>
    public bool HasServersKey { get; set; }
}
