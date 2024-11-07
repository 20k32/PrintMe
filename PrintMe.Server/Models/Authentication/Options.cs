using System.Runtime.InteropServices;
using System.Security;

namespace PrintMe.Server.Models.Authentication;

internal sealed class Options
{
    public SecureString PrivateSecureKey { get; init; }

    /// <summary>
    /// <para>Store string in stack to avoid reading it from RAM by</para>
    /// other programs.
    /// </summary>
    public ReadOnlySpan<char> SecureBase64Span
    {
        get 
        {
            ArgumentNullException.ThrowIfNull(PrivateSecureKey, nameof(PrivateSecureKey));
            
            ReadOnlySpan<char> result;
        
            var valuePtr = IntPtr.Zero;
        
            try 
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(PrivateSecureKey);
                var tempString = Marshal.PtrToStringUni(valuePtr);
                result = tempString.AsSpan();
            } 
            finally 
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
            return result;
        }
    }
}