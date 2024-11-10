using System.Security;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Authentication;
using PrintMe.Server.Models.Authentication;

namespace PrintMe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthorizationController : ControllerBase
{
    private IServiceProvider _provider;
    public AuthorizationController(IServiceProvider provider)
    {
        _provider = provider;
    }
    
    /// <summary>
    /// Generate token for user.
    /// To view use https://jwt.io/
    /// </summary>
    [HttpPost("login")]
    public IResult GenerateToken([FromBody] UserAuthInfo user)
    {
        var generator = _provider.GetService<TokenGenerator>();
        var str = generator.GetForUserInfo(user);
        var json = Results.Json(str);
        
        return json;
    }
}