using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PrintMe.Server.Models.Api.ApiResult.Auth;
using PrintMe.Server.Models.Authentication;

namespace PrintMe.Server.Logic.Authentication;

internal sealed class TokenGenerator
{
    private readonly Options _options;
    public TokenGenerator(Options options) => (_options) = (options);
    
    public string GetForSuccessLoginResult(SuccessLoginResult loginResult)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_options.SecureBase64Span.ToArray());

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = GenerateClaims(loginResult),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = credentials
        };

        var token = handler.CreateToken(tokenDescriptor);
        
        return handler.WriteToken(token);
    }
    
    
    private static ClaimsIdentity GenerateClaims(SuccessLoginResult loginResult)
    {
        var claims = new ClaimsIdentity();
        
        claims.AddClaim(new Claim(CustomClaimTypes.USER_ID, loginResult.Id.ToString()));
        claims.AddClaim(new Claim(ClaimTypes.Email, loginResult.Email));
        claims.AddClaim(new Claim(ClaimTypes.Role, loginResult.Role));
        
        return claims;
    }

}