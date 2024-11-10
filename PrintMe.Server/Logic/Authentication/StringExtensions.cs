using System.Security;

namespace PrintMe.Server.Logic.Authentication;
public static class StringExtensions
{
    public static SecureString ToReadonlySecureString(this string value)
    {
        var secureString = new SecureString();
        
        foreach (var symbol in value)
        {
            secureString.AppendChar(symbol);
        }
        
        secureString.MakeReadOnly();
        
        return secureString;
    }
}