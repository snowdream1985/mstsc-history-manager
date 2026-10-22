using System.Text.Json;
using RemoteDesktopLog.Core.Models;

namespace RemoteDesktopLog.Core.Services;

/// <summary>MRU 列表的 JSON 备份导出/导入。</summary>
public sealed class RdpBackupService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public void Export(string path, IReadOnlyList<string> servers)
    {
        var backup = new MruBackupFile
        {
            ExportedAt = DateTimeOffset.Now,
            Servers = servers.ToList(),
        };
        File.WriteAllText(path, JsonSerializer.Serialize(backup, JsonOptions));
    }

    /// <summary>解析备份文件：逐项 Trim 去空、文件内保序去重。格式无效时抛 <see cref="FormatException"/>。</summary>
    public IReadOnlyList<string> Import(string path)
    {
        MruBackupFile? backup;
        try
        {
            backup = JsonSerializer.Deserialize<MruBackupFile>(File.ReadAllText(path));
        }
        catch (Exception ex) when (ex is JsonException or IOException or UnauthorizedAccessException)
        {
            throw new FormatException("文件格式无效或无法读取，不是本工具导出的备份。", ex);
        }

        if (backup?.Servers is null)
            throw new FormatException("文件格式无效：缺少 servers 列表。");

        var result = new List<string>();
        foreach (var raw in backup.Servers)
        {
            var s = raw?.Trim();
            if (string.IsNullOrEmpty(s))
                continue;
            if (!result.Exists(x => string.Equals(x, s, StringComparison.OrdinalIgnoreCase)))
                result.Add(s);
        }

        if (result.Count == 0)
            throw new FormatException("备份文件中没有任何有效服务器记录。");
        return result;
    }
}
