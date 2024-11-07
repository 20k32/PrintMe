using System.Security;
using System.Security.Cryptography;

namespace PrintMe.Server.Logic.Authentication;

internal static class PrivateKeyGenerator
{
    public static SecureString GetBase64Secure()
    {
        var secureString = new SecureString();
        
        using (var rng = RandomNumberGenerator.Create())
        {
            Span<byte> randomByte = stackalloc byte[1];
            
            foreach (var _ in Enumerable.Range(0, 32))
            {
                rng.GetBytes(randomByte);
                var character = Convert.ToBase64String(randomByte.ToArray())[0];
                secureString.AppendChar(character);
            }
        }
        
        secureString.MakeReadOnly();
        return secureString;
    }
}