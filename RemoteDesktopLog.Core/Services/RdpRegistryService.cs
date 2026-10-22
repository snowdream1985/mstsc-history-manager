using System.Text.RegularExpressions;
using Microsoft.Win32;
using RemoteDesktopLog.Core.Models;

namespace RemoteDesktopLog.Core.Services;

/// <summary>
/// 注册表读写：HKCU\Software\Microsoft\Terminal Server Client 下的
/// Default（MRU 列表）与 Servers（每服务器附加信息）。
/// </summary>
public sealed class RdpRegistryService
{
    public const string DefaultKeyPath = @"Software\Microsoft\Terminal Server Client\Default";
    public const string ServersKeyPath = @"Software\Microsoft\Terminal Server Client\Servers";

    private static readonly Regex MruNameRegex = new(@"^MRU(\d+)$", RegexOptions.Compiled);

    /// <summary>按 MRU 编号升序返回列表（容忍编号缺口与非 REG_SZ 值）。键不存在时返回空列表。</summary>
    public IReadOnlyList<string> ReadMruList()
    {
        using var key = Registry.CurrentUser.OpenSubKey(DefaultKeyPath, writable: false);
        if (key is null)
            return Array.Empty<string>();

        var numbered = new SortedDictionary<int, string>();
        foreach (var valueName in key.GetValueNames())
        {
            var m = MruNameRegex.Match(valueName);
            if (!m.Success || !int.TryParse(m.Groups[1].Value, out var index))
                continue;
            if (key.GetValue(valueName) is string s && !string.IsNullOrWhiteSpace(s))
                numbered[index] = s;
        }
        return numbered.Values.ToList();
    }

    /// <summary>MRU 编号是否从 0 连续（不连续时任何写操作都会自动重排）。</summary>
    public bool IsMruNumberingContiguous()
    {
        using var key = Registry.CurrentUser.OpenSubKey(DefaultKeyPath, writable: false);
        if (key is null)
            return true;

        var indexes = key.GetValueNames()
            .Select(n => MruNameRegex.Match(n))
            .Where(m => m.Success)
            .Select(m => int.Parse(m.Groups[1].Value))
            .OrderBy(i => i)
            .ToList();

        return indexes.Select((v, i) => v == i).All(x => x);
    }

    /// <summary>读取 Servers\host 子键的用户名提示与证书指纹状态。子键不存在或 host 含非法字符时返回 (null, false)。</summary>
    public (string? UsernameHint, bool HasCertHash) ReadServerMeta(string host)
    {
        // 反斜杠在注册表路径中是分隔符，含它的名称不可能对应有效子键
        if (string.IsNullOrWhiteSpace(host) || host.Contains('\\'))
            return (null, false);

        using var key = Registry.CurrentUser.OpenSubKey($@"{ServersKeyPath}\{host}", writable: false);
        if (key is null)
            return (null, false);

        var hint = key.GetValue("UsernameHint") as string;
        var hasCert = key.GetValue("CertHash") is byte[] bytes && bytes.Length > 0;
        return (string.IsNullOrWhiteSpace(hint) ? null : hint, hasCert);
    }

    /// <summary>Servers 下是否存在对应子键。</summary>
    public bool ServerKeyExists(string host)
    {
        if (string.IsNullOrWhiteSpace(host) || host.Contains('\\'))
            return false;
        using var key = Registry.CurrentUser.OpenSubKey($@"{ServersKeyPath}\{host}", writable: false);
        return key is not null;
    }

    /// <summary>枚举 Servers 全部子键的元信息（不判断是否孤儿，由调用方对照 MRU 判定）。</summary>
    public IReadOnlyList<OrphanServerInfo> ReadAllServerKeys()
    {
        using var key = Registry.CurrentUser.OpenSubKey(ServersKeyPath, writable: false);
        if (key is null)
            return Array.Empty<OrphanServerInfo>();

        var result = new List<OrphanServerInfo>();
        foreach (var subName in key.GetSubKeyNames())
        {
            var meta = ReadServerMeta(subName);
            result.Add(new OrphanServerInfo
            {
                KeyName = subName,
                UsernameHint = meta.UsernameHint,
                HasCertHash = meta.HasCertHash,
            });
        }
        return result;
    }

    /// <summary>
    /// 唯一 MRU 写入口：先检测 mstsc（运行则抛 <see cref="MstscRunningException"/>），
    /// 再删除全部 MRUx 值并连续重写 MRU0..n —— 一步同时完成重排编号与清除尾部残留。
    /// </summary>
    public void WriteMruList(IReadOnlyList<string> ordered)
    {
        if (MstscProcessGuard.IsMstscRunning())
            throw new MstscRunningException();

        using var key = Registry.CurrentUser.CreateSubKey(DefaultKeyPath, writable: true);

        foreach (var valueName in key.GetValueNames().Where(n => MruNameRegex.IsMatch(n)))
            key.DeleteValue(valueName, throwOnMissingValue: false);

        for (var i = 0; i < ordered.Count; i++)
            key.SetValue($"MRU{i}", ordered[i], RegistryValueKind.String);
    }

    /// <summary>删除单个 Servers 子键。同样受 mstsc 防护；失败返回 false 并给出原因，不抛异常。</summary>
    public bool TryDeleteServerKey(string keyName, out string? error)
    {
        error = null;
        if (string.IsNullOrWhiteSpace(keyName) || keyName.Contains('\\'))
        {
            error = "子键名无效";
            return false;
        }
        if (MstscProcessGuard.IsMstscRunning())
        {
            error = "mstsc 正在运行";
            return false;
        }

        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(ServersKeyPath, writable: true);
            if (key is null)
                return true; // 父键都不存在，视为已删除
            key.DeleteSubKeyTree(keyName, throwOnMissingSubKey: false);
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }
}
