using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Authentication;
using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Logic.Services.Database.Interfaces;
using PrintMe.Server.Models.Api;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.Exceptions;


namespace PrintMe.Server.Controllers;

[ApiController]
[Route("api/Auth")]
public sealed class AuthorizationController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthorizationController(IServiceProvider provider)
    {
        _userService = provider.GetService<IUserService>();
    }

    /// <summary>
    /// Checks for user in database and generates token with fields: id, email, role.
    /// To view use https://jwt.io/
    /// </summary>
    [ProducesResponseType(typeof(ApiResult<JwtResult>), 200)]
    [HttpPost("Login")]
    public async Task<IActionResult> GenerateToken([FromBody] UserAuthRequest authRequest)
    {
        PlainResult result;

        if (authRequest is null)
        {
            result = new("Missing body.",
                StatusCodes.Status403Forbidden);
        }
        else if (authRequest.IsNull())
        {
            result = new("Missing parameters in body.",
                StatusCodes.Status403Forbidden);
        }
        else
        {
            try
            {
                var token = await _userService.GenerateTokenAsync(authRequest);
                result = new ApiResult<JwtResult>(token, "Token successfully created.",
                    StatusCodes.Status200OK);
            }
            catch (NoRoleAvailableException ex)
            {
                result = new(ex.Message, StatusCodes.Status405MethodNotAllowed);
            }
            catch (NotFoundUserInDbException ex)
            {
                result = new(ex.Message, StatusCodes.Status404NotFound);
            }
            catch (IncorrectPasswordException ex)
            {
                result = new(ex.Message, StatusCodes.Status403Forbidden);
            }
            catch (Exception ex)
            {
                result = new($"Internal server error while generating token for user.\n{ex.Message}\n{ex.StackTrace}",
                    StatusCodes.Status500InternalServerError);
            }
        }

        return result.ToObjectResult();
    }

    /// <summary>
    /// Create a new user and save it to the database.
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequest userRegistration)
    {
        PlainResult result = null;
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
            catch (NotFoundUserInDbException)
            {
                try
                {
                    await _userService.AddUserAsync(userRegistration);
                    result = new("Successfully created", StatusCodes.Status200OK);
                }
                catch (InvalidEmailFormatException e)
                {
                    result = new(e.Message, StatusCodes.Status409Conflict);
                }
            }
            catch (Exception ex)
            {
                result = new($"Internal server error while registering user.\n{ex.Message}\n{ex.StackTrace}",
                    StatusCodes.Status500InternalServerError);
            }
        }

        if (result != null)
        {
            return result.ToObjectResult();
        }

        return new PlainResult("Internal server error while registering user.",
                StatusCodes.Status500InternalServerError)
            .ToObjectResult();
    }
    
    /// <summary>
    /// Refreshes JWT token (access token).
    /// </summary>
    [HttpPost("refreshToken")]
    [ProducesResponseType(typeof(ApiResult<JwtResult>), 200)]
    public async Task<IActionResult> RefreshToken([FromBody] JwtResult jwtResult)
    {
        PlainResult result = null;
        
        if (jwtResult is null)
        {
            result = new("Missing body.", StatusCodes.Status403Forbidden);
        }
        else if (jwtResult.IsNull())
        {
            result = new("Missing parameters in body.", StatusCodes.Status403Forbidden);
        }
        else
        {
            var newToken = await _userService.RefreshTokenAsync(jwtResult);

            if (newToken is not null)
            {
                result = new ApiResult<JwtResult>(newToken, "token refrehsed successfully",
                    StatusCodes.Status200OK);
            }
            else
            {
                throw new DatabaseInternalException();
            }
        }

        if (result is not null)
        {
            return result.ToObjectResult();
        }

        return new PlainResult("Internal server error while registering user.",
                StatusCodes.Status500InternalServerError)
            .ToObjectResult();
    }
}