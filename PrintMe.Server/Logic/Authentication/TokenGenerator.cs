using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PrintMe.Server.Models.Authentication;

namespace PrintMe.Server.Logic.Authentication;

internal sealed class TokenGenerator
{
    private readonly Options _options;
    public TokenGenerator(Options options) => (_options) = (options);
    
    public string GetForUserInfo(UserAuthInfo user)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_options.SecureBase64Span.ToArray());

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = GenerateClaims(user),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = credentials
        };

        var token = handler.CreateToken(tokenDescriptor);
        
        return handler.WriteToken(token);
    }
    
    
    private static ClaimsIdentity GenerateClaims(UserAuthInfo user)
    {
        var claims = new ClaimsIdentity();
        
        claims.AddClaim(new Claim(ClaimTypes.Name, user.Name));
        claims.AddClaim(new Claim(ClaimTypes.Role, user.Role));
        
        return claims;
    }

}