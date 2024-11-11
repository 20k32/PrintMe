using System.Security.Cryptography;

namespace PrintMe.Server.Logic.Registration;

public class SecurityHelper
{
    private const int SaltLenght = 128;
    private const int HashIterations = 1024;

    public static string GenerateSalt()
    {
        var saltBytes = new byte[SaltLenght];

        using (var provider = RandomNumberGenerator.Create())
        {
            provider.GetNonZeroBytes(saltBytes);
        }

        return Convert.ToBase64String(saltBytes);
    }

    public static string HashPassword(string password, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);

        using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, HashIterations, HashAlgorithmName.SHA256))
        {
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(HashIterations));
        }
    }
    
    public static bool VerifyPassword(string password, string hashedPassword, string salt)
    {
        var newHashed = HashPassword(password, salt);
        return newHashed.Equals(hashedPassword);
    }
}