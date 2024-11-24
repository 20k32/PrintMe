using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.Api.ApiResult;
using PrintMe.Server.Models.Api.ApiResult.Auth;

using PrintMe.Server.Models.Exceptions;


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
    [HttpPost("register")]
    public async Task<IResult> RegisterUser([FromBody]UserRegisterRequest userRegistration)
    {
        ResultBase result = null;
        if (userRegistration is null)
        {
            result = new("Missing body.", StatusCodes.Status403Forbidden);
        }
        else if (userRegistration.IsNull())
        {
            result = new("Missing parameters in body.", StatusCodes.Status403Forbidden);
        }
        else
        {
            try
            {
                if (await _userService.GetUserByEmailAsync(userRegistration.Email) != null)
                {
                    result = new("User with this email already exists.", StatusCodes.Status409Conflict);
                }
            }
            catch (NotFoundUserInDbException ex)
            {
                try
                {
                    await _userService.AddUserAsync(userRegistration);
                    result = new("User registered successfully.", StatusCodes.Status200OK);
                }
                catch (InvalidEmailFormatException e)
                {
                    result = new(e.Message, StatusCodes.Status409Conflict);
                }
            }
            catch(Exception ex)
            {
                result = new($"Internal server error while registering user.\n{ex.Message}\n{ex.StackTrace}",
                    StatusCodes.Status500InternalServerError);
            }
        }
        return Results.Json(result);
    }
}