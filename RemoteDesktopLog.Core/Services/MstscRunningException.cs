namespace RemoteDesktopLog.Core.Services;

/// <summary>写注册表前检测到 mstsc.exe 正在运行时抛出（Core 层兜底防护）。</summary>
public sealed class MstscRunningException : InvalidOperationException
{
    public MstscRunningException()
        : base("检测到远程桌面连接 (mstsc.exe) 正在运行，它退出时会覆盖对历史记录的修改。请先关闭所有远程桌面窗口。")
    {
    }
}
