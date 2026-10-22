using System.Runtime.InteropServices;

namespace RemoteDesktopLog.Core.Services;

/// <summary>
/// Windows 凭据管理器中的 TERMSRV/* 凭据（mstsc 保存的密码）。
/// 用 P/Invoke 而非解析 cmdkey 输出 —— cmdkey 输出是本地化文本，不可靠。
/// 实测 TERMSRV 凭据类型为 CRED_TYPE_DOMAIN_PASSWORD(2)，删除时按枚举返回的 Type 执行。
/// </summary>
public sealed class CredentialService
{
    private const int CredTypeGeneric = 1;
    private const int CredTypeDomainPassword = 2;
    private const int ErrorNotFound = 1168;
    private const string TermsrvPrefix = "TERMSRV/";

    /// <summary>枚举全部 TERMSRV/* 凭据，返回 主机名 → 凭据类型 的映射（键比较 IgnoreCase）。</summary>
    public IReadOnlyDictionary<string, int> EnumerateTermsrvTargets()
    {
        var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        if (!CredEnumerateW(null, 0, out var count, out var buffer))
        {
            // ERROR_NOT_FOUND = 没有任何凭据，属正常情况
            if (Marshal.GetLastWin32Error() == ErrorNotFound)
                return result;
            return result;
        }

        try
        {
            for (var i = 0; i < count; i++)
            {
                var credPtr = Marshal.ReadIntPtr(buffer, i * IntPtr.Size);
                var cred = Marshal.PtrToStructure<CREDENTIALW>(credPtr);
                var target = Marshal.PtrToStringUni(cred.TargetName);
                if (target is not null && target.StartsWith(TermsrvPrefix, StringComparison.OrdinalIgnoreCase))
                    result[target[TermsrvPrefix.Length..]] = cred.Type;
            }
        }
        finally
        {
            CredFree(buffer);
        }
        return result;
    }

    /// <summary>
    /// 删除 TERMSRV/&lt;host&gt; 凭据。优先使用枚举缓存的类型（实测为域密码类型 2），
    /// 缓存未命中时依次尝试域密码、普通两种类型。凭据本就不存在视为成功。
    /// </summary>
    public bool DeleteCredential(string host)
    {
        var target = TermsrvPrefix + host;
        var knownTypes = EnumerateTermsrvTargets();

        var typesToTry = knownTypes.TryGetValue(host, out var type)
            ? new[] { type }
            : new[] { CredTypeDomainPassword, CredTypeGeneric };

        var lastError = 0;
        foreach (var t in typesToTry.Distinct())
        {
            if (CredDeleteW(target, t, 0))
                return true;
            lastError = Marshal.GetLastWin32Error();
            if (lastError == ErrorNotFound)
                return true; // 本来就没有，视为成功
        }
        return false;
    }

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool CredEnumerateW(string? filter, int flags, out int count, out IntPtr credentials);

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool CredFree(IntPtr buffer);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool CredDeleteW(string targetName, int type, int flags);

    [StructLayout(LayoutKind.Sequential)]
    private struct CREDENTIALW
    {
        public int Flags;
        public int Type;
        public IntPtr TargetName;
        public IntPtr Comment;
        public long LastWritten;
        public int CredentialBlobSize;
        public IntPtr CredentialBlob;
        public int Persist;
        public int AttributeCount;
        public IntPtr Attributes;
        public IntPtr TargetAlias;
        public IntPtr UserName;
    }
}
