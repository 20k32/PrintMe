using System.Security;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Authentication;
using PrintMe.Server.Logic.Registration;
using PrintMe.Server.Models.ApiResult;
using PrintMe.Server.Models.ApiResult.Common;
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
    public IResult GenerateToken([FromBody] UserAuthRequest user)
    {
        ResultBase resultBase = null; 
        var context = _provider.GetService<PrintMeDbContext>();
        
        var dbUser = context.Users
            .AsQueryable()
            .FirstOrDefault(existing => existing.Email.Equals(user.Email));

        if (dbUser is null)
        {
            resultBase = new("There is no such use in database.", StatusCodes.Status403Forbidden);
        }
        else if (!dbUser.Password.Equals(user.Password))
        {
            resultBase = new("Password is incorrect, please, try another one.",
                StatusCodes.Status401Unauthorized);
        }
        else
        {
            var generator = _provider.GetService<TokenGenerator>();
            var token = generator.GetForUserInfo(user);
            resultBase = new("There is such user in database.", StatusCodes.Status200OK);
        }
        
        var json = Results.Json(resultBase);
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