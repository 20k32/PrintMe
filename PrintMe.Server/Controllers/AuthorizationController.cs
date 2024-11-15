using System.Security;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Authentication;
using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.Api.ApiResult.Auth;
using PrintMe.Server.Models.Authentication;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Persistence;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository;

namespace PrintMe.Server.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthorizationController : ControllerBase
{
    private readonly UserService _userService;
    public AuthorizationController(IServiceProvider provider)
    {
        _userService = provider.GetService<UserService>();
    }
    
    /// <summary>
    /// Checks for user in database and generates token with fields: id, email, role.
    /// To view use https://jwt.io/
    /// </summary>
    [ProducesResponseType(typeof(TokenResult), 200)]
    [HttpPost("login")]
    public async Task<IResult> GenerateToken([FromBody] UserAuthRequest authRequest)
    {
        TokenResult result;
        
        if (authRequest is null)
        {
            result = new(null, "Missing body.", 
                StatusCodes.Status403Forbidden);
        }
        else if (authRequest.IsNull())
        {
            result = new(null, "Missing parameters in body.", 
                StatusCodes.Status403Forbidden);
        }
        else
        {
            try
            {
                var token = await _userService.GenerateTokenAsync(authRequest);
                result = new(token, "Token successfully created.",
                    StatusCodes.Status200OK);
            }
            catch (NotFoundUserInDbException ex)
            {
                result = new(null, ex.Message, StatusCodes.Status404NotFound);
            }
            catch (IncorrectPasswordException ex)
            {
                result = new(null, ex.Message, StatusCodes.Status403Forbidden);
            }
            catch (Exception ex)
            {
                result = new(null, $"Internal server error while generating token for user.\n{ex.Message}\n{ex.StackTrace}",
                    StatusCodes.Status500InternalServerError);
            }
        }

        return Results.Json(result);
    }
    
    /// <summary>
    /// Create a new user and save it to the database.
    /// </summary>
    [HttpPost("registration")]
    public IResult RegisterUser()
    {
        /*var context = _provider.GetService<PrintMeDbContext>();

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
        
        return Results.Ok(new { message = "User registered successfully" });*/

        return Results.Empty;
    }
}