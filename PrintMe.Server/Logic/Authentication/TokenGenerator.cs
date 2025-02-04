using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PrintMe.Server.Models.Authentication;
using PrintMe.Server.Models.Exceptions;

namespace PrintMe.Server.Logic.Authentication;

internal sealed class TokenGenerator
{
    private readonly Options _options;
    public TokenGenerator(Options options) => (_options) = (options);
    
    public string GetForSuccessLoginResult(SuccessLoginEntity loginEntity)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_options.SecureBase64Span.ToArray());

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = GenerateClaims(loginEntity),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = credentials
        };

        var token = handler.CreateToken(tokenDescriptor);
        
        
        return handler.WriteToken(token);
    }
    
    
    private ClaimsIdentity GenerateClaims(SuccessLoginEntity loginEntity)
    {
        var claims = new ClaimsIdentity();
        
        claims.AddClaim(new Claim(CustomClaimTypes.USER_ID, loginEntity.Id.ToString()));
        claims.AddClaim(new Claim(CustomClaimTypes.EMAIL, loginEntity.Email));
        claims.AddClaim(new Claim(CustomClaimTypes.ROLE, loginEntity.Role));
        
        return claims;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        
        return Convert.ToBase64String(randomNumber);
    }
    
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, 
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecureBase64Span.ToArray())),
            ValidateLifetime = false, 
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new InvalidTokenException();
        }
        
        return principal;
    }
}