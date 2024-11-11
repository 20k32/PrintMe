using System.Security;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Entities;
using PrintMe.Server.Logic.Authentication;
using PrintMe.Server.Logic.Registration;
using PrintMe.Server.Models.Authentication;
using PrintMe.Server.Models.Registration;
using PrintMe.Server.Persistence.Registration;

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
    
    /// <summary>
    /// Create a new user and save it to the database.
    /// </summary>
    [HttpPost("registration")]
    public IResult RegisterUser([FromBody] UserRegistrationInfo userData)
    {
        var _context = _provider.GetService<UserContext>();
        var _userPersistence = new UserPersistence(_context);
        UserRegistrationInfo userInfo =  UserRegistrationLogic.CreateUser(userData.Email, userData.HashedPassword, userData.FirstName, userData.LastName);
        _userPersistence.SaveUser(userInfo);
        return Results.Ok(new { message = "User registered successfully" });
    }
}