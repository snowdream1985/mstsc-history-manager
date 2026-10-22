namespace RemoteDesktopLog.Core.Services;

/// <summary>从 MRU 字符串中提取主机名（用于关联 Servers 子键与 TERMSRV 凭据）。</summary>
public static class ServerNameParser
{
    /// <summary>
    /// "[::1]:3389" → "::1"；"host:端口"（仅一个冒号且其后全为数字）→ "host"；其余原样返回。
    /// </summary>
    public static string ExtractHost(string serverName)
    {
        if (string.IsNullOrWhiteSpace(serverName))
            return serverName;

        var s = serverName.Trim();

        // IPv6 带端口：[::1]:3389
        if (s.StartsWith('['))
        {
            var close = s.IndexOf(']');
            if (close > 0)
                return s.Substring(1, close - 1);
            return s;
        }

        // host:port —— 恰好一个冒号且冒号后全是数字才视为端口
        var firstColon = s.IndexOf(':');
        if (firstColon > 0 && firstColon == s.LastIndexOf(':'))
        {
            var tail = s[(firstColon + 1)..];
            if (tail.Length > 0 && tail.All(char.IsDigit))
                return s[..firstColon];
        }

        return s;
    }
}
