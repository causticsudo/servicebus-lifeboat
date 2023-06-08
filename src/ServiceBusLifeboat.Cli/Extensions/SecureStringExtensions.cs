using System.Runtime.InteropServices;
using System.Security;

namespace ServiceBusLifeboat.Cli.Extensions;

public static class SecureStringExtensions
{
    public static string ToStringSafely(this SecureString secureString)
    {
        var insecureStringPtr = IntPtr.Zero;

        try
        {
            insecureStringPtr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
            return Marshal.PtrToStringUni(insecureStringPtr);
        }
        finally
        {
            Marshal.ZeroFreeGlobalAllocUnicode(insecureStringPtr);
        }
    }
}