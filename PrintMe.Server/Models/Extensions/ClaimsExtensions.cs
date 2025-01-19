using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PrintMe.Server.Logic.Authentication;

namespace PrintMe.Server.Models.Extensions
{
    public static class ClaimsExtensions
    {
        public static string TryGetUserId(this HttpRequest request)
        {
            var jwtRaw = request.Headers.Authorization;
            var jwtToken = jwtRaw.First().Substring(7);

            var jwtHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)jwtHandler.ReadToken(jwtToken);
            var id = securityToken.Claims.FirstOrDefault(existing
                => existing.Type.Equals(CustomClaimTypes.USER_ID))?.Value;

            return id;
        }
        
        public static string TryGetUserRole(this HttpRequest request)
        {
            var jwtRaw = request.Headers.Authorization;
            var jwtToken = jwtRaw.First().Substring(7);

            var jwtHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)jwtHandler.ReadToken(jwtToken);
            var id = securityToken.Claims.FirstOrDefault(existing
                => existing.Type.Equals(CustomClaimTypes.ROLE))?.Value;

            return id;
        }
    }
}