using System.Diagnostics;

namespace RemoteDesktopLog.Core.Services;

/// <summary>mstsc 进程检测：mstsc 运行期间它退出时会用内存中的旧 MRU 回写注册表，覆盖外部修改。</summary>
public static class MstscProcessGuard
{
    public static bool IsMstscRunning()
    {
        try
        {
            return Process.GetProcessesByName("mstsc").Length > 0;
        }
        catch
        {
            // 进程枚举失败时按"未运行"处理，不阻断用户操作
            return false;
        }
    }
}
