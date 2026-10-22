namespace RemoteDesktopLog.Core.Services;

/// <summary>MRU 列表的纯内存操作（零依赖，便于单元测试）。所有比较按 OrdinalIgnoreCase。</summary>
public static class MruListEditor
{
    /// <summary>去重后插入顶部（已存在则提升到顶部），与 mstsc 行为一致。</summary>
    public static void AddToTop(List<string> list, string name)
    {
        list.RemoveAll(s => string.Equals(s, name, StringComparison.OrdinalIgnoreCase));
        list.Insert(0, name);
    }

    /// <summary>按索引集合删除（索引基于删除前的列表）。</summary>
    public static void RemoveAt(List<string> list, IReadOnlyList<int> indexes)
    {
        foreach (var i in indexes.OrderByDescending(x => x))
        {
            if (i >= 0 && i < list.Count)
                list.RemoveAt(i);
        }
    }

    /// <summary>将 index 处元素移动 delta 位（-1 上移 / +1 下移）。越界返回 false 不做改动。</summary>
    public static bool Move(List<string> list, int index, int delta)
    {
        var target = index + delta;
        if (index < 0 || index >= list.Count || target < 0 || target >= list.Count)
            return false;
        (list[index], list[target]) = (list[target], list[index]);
        return true;
    }

    /// <summary>合并：现有列表原序保留，导入中不存在的新项按导入文件顺序追加尾部。</summary>
    public static List<string> Merge(IReadOnlyList<string> existing, IReadOnlyList<string> imported)
    {
        var result = new List<string>(existing);
        foreach (var item in imported)
        {
            if (!result.Exists(s => string.Equals(s, item, StringComparison.OrdinalIgnoreCase)))
                result.Add(item);
        }
        return result;
    }
}
