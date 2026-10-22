using System.Text.RegularExpressions;

namespace RemoteDesktopLog.Core.Services;

/// <summary>简单通配符匹配：* 匹配任意长度，? 匹配单字符，不区分大小写。</summary>
public static class WildcardMatcher
{
    public static bool IsMatch(string input, string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
            return false;
        var regex = "^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
        return Regex.IsMatch(input, regex, RegexOptions.IgnoreCase);
    }

    /// <summary>返回列表中匹配 pattern 的项的索引。</summary>
    public static IReadOnlyList<int> MatchIndexes(IReadOnlyList<string> list, string pattern)
    {
        var result = new List<int>();
        for (var i = 0; i < list.Count; i++)
        {
            if (IsMatch(list[i], pattern))
                result.Add(i);
        }
        return result;
    }
}
