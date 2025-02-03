using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Models.Api;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs.UserDto;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Models.Extensions;

namespace PrintMe.Server.Controllers
{
    [ApiController]
    [Route("api/Users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly VerificationService _verificationService;
        
        public UserController(IServiceProvider provider)
        {
            _userService = provider.GetService<UserService>();
            _verificationService = provider.GetService<VerificationService>();
        }
        
        /// <summary>
        /// Checks for user in database and returns him if it is present.
        /// </summary>
        [ProducesResponseType(typeof(ApiResult<PasswordUserDto>), 200)]
        [HttpGet("{id?}")]
        public async Task<IActionResult> GetUserById(int? id)
        {
            PlainResult result;

            if (id is null)
            {
                result = new("Missing id.",
                    StatusCodes.Status401Unauthorized);
            }
            else
            {
                try
                {
                    var userDto = await _userService.GetUserByIdAsync(id.Value);
                    
                    result = new ApiResult<PasswordUserDto>(userDto, "There is such user in database", 
                        StatusCodes.Status200OK);
                }
                catch (NotFoundUserInDbException ex)
                {
                    result = new (ex.Message,
                        StatusCodes.Status404NotFound);
                }
                catch (Exception ex)
                {
                    result = new ($"Internal server error while getting user\n{ex.Message}\n{ex.StackTrace}",
                        StatusCodes.Status500InternalServerError);
                }
            }
            
            return result.ToObjectResult();
        }

        
        /// <summary>
        /// Updates user fields except password because it requires additional logic.
        /// </summary>
        [ProducesResponseType(typeof(ApiResult<PasswordUserDto>), 200)]
        [HttpPut("user")]
        public async Task<IActionResult> UpdateUserById([FromBody] NoPasswordUserDto noPasswordUser)
        {
            PlainResult result;

            if (noPasswordUser is null)
            {
                result = new ( "Missing body.", StatusCodes.Status400BadRequest);
            }
            else if (noPasswordUser.IsNull())
            {
                result = new ("Missing parameters in body.", StatusCodes.Status400BadRequest);
            }
            else
            {
                try
                {
                    var user = await _userService.UpdateUser(noPasswordUser.UserId, noPasswordUser);
                    result = new ApiResult<PasswordUserDto>(user, "There is such user in database.",
                        StatusCodes.Status200OK);
                }
                catch (NotFoundUserInDbException ex)
                {
                    result = new(ex.Message, StatusCodes.Status403Forbidden);
                }
                catch (Exception ex)
                {
                    result = new($"Internal server error while updating user.\n{ex.Message}\n{ex.StackTrace}",
                        StatusCodes.Status500InternalServerError);
                }
            }
            
            return result.ToObjectResult();
        }
        
        /// <summary>
        /// Generates new salt and new password for a user if it exists.
        /// </summary>
        [HttpPut("password")]
        [ProducesResponseType(typeof(ApiResult<PasswordUserDto>), 200)]
        public async Task<IActionResult> UpdateUserPasswordById([FromBody] UpdatePasswordRequest updatePasswordRequest)
        {
            PlainResult result;
            
            if (updatePasswordRequest is null)
            {
                result = new ("Missing parameter from body.", StatusCodes.Status400BadRequest);
            }
            else if (updatePasswordRequest.IsNull())
            {
                result = new ("Missing parameters in body.", StatusCodes.Status400BadRequest);
            }
            else
            {
                try
                {
                    var userWithPassword = await _userService.UpdateUserPasswordAsync(updatePasswordRequest);
                    
                    result = new ApiResult<PasswordUserDto>(userWithPassword, "Credentials successfully updated",
                        StatusCodes.Status200OK);
                }
                catch (NotFoundUserInDbException ex)
                {
                    result = new(ex.Message,
                        StatusCodes.Status404NotFound);
                }
                catch (IncorrectPasswordException ex)
                {
                    result = new(ex.Message,
                        StatusCodes.Status401Unauthorized);
                }
                catch (DatabaseInternalException ex)
                {
                    result = new(ex.Message,
                        StatusCodes.Status500InternalServerError);
                }
                catch (Exception ex)
                {
                    result = new($"Internal server error while updating credentials.\n{ex.Message}\n{ex.StackTrace}",
                        StatusCodes.Status500InternalServerError);
                }
            }

            return result.ToObjectResult();
        }
        
        /// <summary>
        /// Checks for authorized user and returns, parses jwt and return info if he exists.
        /// </summary>
        [Authorize]
        [HttpGet("my")]
        [ProducesResponseType(typeof(ApiResult<PasswordUserDto>), 200)]
        public async Task<IActionResult> GetUserFromJwt()
        {
            int userId;
            
            PlainResult result;
            var id = Request.TryGetUserId();
            
            if (id is null)
            {
                result = new("Missing id.",
                    StatusCodes.Status401Unauthorized);
            }
            else if (!int.TryParse(id, out userId))
            {
                result = new("Unable get id from jwt.",
                    StatusCodes.Status401Unauthorized);
            }
            else
            {
                try
                {
                    var userDto = await _userService.GetUserByIdAsync(userId);
                    
                    result = new ApiResult<PasswordUserDto>(userDto, "There is such user in database", 
                        StatusCodes.Status200OK);
                }
                catch (NotFoundUserInDbException ex)
                {
                    result = new(ex.Message,
                        StatusCodes.Status404NotFound);
                }
                catch (Exception ex)
                {
                    result = new ($"Internal server error while getting user\n{ex.Message}\n{ex.StackTrace}",
                        StatusCodes.Status500InternalServerError);
                }
            }

            return result.ToObjectResult();
        }
        
        /// <summary>
        /// Updates your password.
        /// </summary>
        [Authorize]
        [HttpPatch("my/password")]
        [ProducesResponseType(typeof(ApiResult<PasswordUserDto>), 200)]
        public async Task<IActionResult> UpdateUserPasswordFromJwt([FromBody] MyPasswordUpdateRequest passwordRequest)
        {
            PlainResult result;
            
            if (passwordRequest is null)
            {
                result = new ApiResult<PasswordUserDto>(null, "Missing parameter from body.", StatusCodes.Status400BadRequest);
            }
            else if (passwordRequest.IsNull())
            {
                result = new ApiResult<PasswordUserDto>(null, "Missing parameters in body.", StatusCodes.Status400BadRequest);
            }
            else
            {
                var id = Request.TryGetUserId();
                int userId;
                if (id is null)
                {
                    result = new("Missing id.",
                        StatusCodes.Status401Unauthorized);
                }
                else if (!int.TryParse(id, out userId))
                {
                    result = new("Unable get id from jwt.",
                        StatusCodes.Status401Unauthorized);
                }
                else
                {
                    try
                    {
                        var userWithPassword = await _userService.UpdateUserPasswordAsync(userId, passwordRequest);
                        result = new ApiResult<PasswordUserDto>(userWithPassword, "Credentials successfully updated",
                            StatusCodes.Status200OK);
                    }
                    catch (NotFoundUserInDbException ex)
                    {
                        result = new(ex.Message,
                            StatusCodes.Status404NotFound);
                    }
                    catch (IncorrectPasswordException ex)
                    {
                        result = new(ex.Message,
                            StatusCodes.Status401Unauthorized);
                    }
                    catch (DatabaseInternalException ex)
                    {
                        result = new(ex.Message,
                            StatusCodes.Status500InternalServerError);
                    }
                    catch (Exception ex)
                    {
                        result = new($"Internal server error while updating credentials.\n{ex.Message}\n{ex.StackTrace}",
                            StatusCodes.Status500InternalServerError);
                    }
                }
            }

            return result.ToObjectResult();
        }
        
        /// <summary>
        /// Sends an email verification to the user email.
        /// </summary>
        [ProducesResponseType(typeof(ApiResult<string>), 200)]
        [HttpGet("sendverificationmail")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SendEmailVerification()
        {
            PlainResult result;
        
            try
            {
                if (!int.TryParse(Request.TryGetUserId(), out var id))
                {
                    throw new ArgumentException("Invalid user ID");
                }
                await _verificationService.SendEmailVerificationAsync(id);
                result = new PlainResult("Email verification successfully sent.", StatusCodes.Status200OK);
            }
            catch (NoRoleAvailableException ex)
            {
                return StatusCode(StatusCodes.Status405MethodNotAllowed, new PlainResult(ex.Message, StatusCodes.Status405MethodNotAllowed));
            }
            catch (NotFoundUserInDbException ex)
            {
                return NotFound(new PlainResult(ex.Message, StatusCodes.Status404NotFound));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new PlainResult(ex.Message, StatusCodes.Status400BadRequest));
            }
            catch (SmtpFailedRecipientException ex)
            {
                return BadRequest(new PlainResult($"Failed to deliver email to recipient.\n{ex.Message}", StatusCodes.Status400BadRequest));
            }
            catch (SmtpException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new PlainResult($"SMTP error occurred while sending email.\n{ex.Message}\n{ex.StackTrace}", StatusCodes.Status500InternalServerError));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new PlainResult($"Internal server error while sending email.\n{ex.Message}", StatusCodes.Status500InternalServerError));
            }
        
            return result.ToObjectResult();
        }
        
        
        /// <summary>
        /// Email verification by UUID token.
        /// </summary>
        [ProducesResponseType(typeof(ApiResult<string>), 200)]
        [HttpPatch("verifyemail")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            PlainResult result;

            try
            {
                await _verificationService.VerifyEmailAsync(token);
                result = new PlainResult("Email verified successfully.", StatusCodes.Status200OK);
            }
            catch (InvalidUUIDTokenException ex)
            {
                return BadRequest(new PlainResult(ex.Message, StatusCodes.Status400BadRequest));
            }
            catch (NotFoundUserInDbException ex)
            {
                return NotFound(new PlainResult(ex.Message, StatusCodes.Status404NotFound));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new PlainResult(ex.Message, StatusCodes.Status400BadRequest));
            }

            return result.ToObjectResult();
        }
    }
}