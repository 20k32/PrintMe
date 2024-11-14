using System.Security;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Authentication;
using PrintMe.Server.Logic.Registration;
using PrintMe.Server.Models.Authentication;
using PrintMe.Server.Persistence;
using PrintMe.Server.Persistence.Models;

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
    public IResult RegisterUser()
    {
        var context = _provider.GetService<PrintMeDbContext>();

        var userInfo = new User()
        {
            FirstName = "1",
            LastName = "2",
            Email = "123@123.com",
            PhoneNumber = "3",
            UserStatusId = 1,
            ShouldHidePhoneNumber = false,
            Description = "1",
            Password = "31"
        };
        context.Users.Add(userInfo);
        context.SaveChanges();

        var user = context.Users.First();
        
        return Results.Ok(new { message = "User registered successfully" });
    }
}