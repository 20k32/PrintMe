using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.Api.ApiResult.Common;
using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Models.Extensions;
using PrintMe.Server.Persistence;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository;

namespace PrintMe.Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        
        public UserController(IServiceProvider provider)
        {
            _userService = provider.GetService<UserService>();
        }
        
        /// <summary>
        /// Checks for user in database and returns him if it is present.
        /// </summary>
        [ProducesResponseType(typeof(UserResult), 200)]
        [HttpGet("{id?}")]
        public async Task<IResult> GetUserById(int? id)
        {
            UserResult result;

            if (id is null)
            {
                result = new(null, "Missing id.",
                    StatusCodes.Status401Unauthorized);
            }
            else
            {
                try
                {
                    var userDto = await _userService.GetUserByIdAsync(id.Value);
                    
                    result = new(userDto, "There is such user in database", 
                        StatusCodes.Status200OK);
                }
                catch (NotFoundUserInDbException ex)
                {
                    result = new(null, ex.Message,
                        StatusCodes.Status404NotFound);
                }
                catch (Exception ex)
                {
                    result = new(null, $"Internal server error while getting user\n{ex.Message}\n{ex.StackTrace}",
                        StatusCodes.Status500InternalServerError);
                }
            }
            
            return Results.Json(result);
        }

        
        /// <summary>
        /// Updates user fields except password because it requires additional logic.
        /// </summary>
        [ProducesResponseType(typeof(UserResult), 200)]
        [HttpPut("user")]
        public async Task<IResult> UpdateUserById([FromBody] NoPasswordUserDto noPasswordUser)
        {
            UserResult result;

            if (noPasswordUser is null)
            {
                result = new UserResult(null, "Missing body.", StatusCodes.Status400BadRequest);
            }
            else if (noPasswordUser.IsNull())
            {
                result = new UserResult(null, "Missing parameters in body.", StatusCodes.Status400BadRequest);
            }
            else
            {
                try
                {
                    var user = await _userService.UpdateUser(noPasswordUser.UserId, noPasswordUser);
                    
                    result = new(user, "There is such user in database.",
                        StatusCodes.Status200OK);
                }
                catch (NotFoundUserInDbException ex)
                {
                    result = new(null, ex.Message, StatusCodes.Status403Forbidden);
                }
                catch (Exception ex)
                {
                    result = new(null, $"Internal server error while updating user.\n{ex.Message}\n{ex.StackTrace}",
                        StatusCodes.Status500InternalServerError);
                }
            }
            
            return Results.Json(result);
        }
        
        /// <summary>
        /// Generates new salt and new password for a user if it exists.
        /// </summary>
        [HttpPut("password")]
        [ProducesResponseType(typeof(UserResult), 200)]
        public async Task<IResult> UpdateUserPasswordById([FromBody] UpdatePasswordRequest updatePasswordRequest)
        {
            UserResult result;
            
            if (updatePasswordRequest is null)
            {
                result = new UserResult(null, "Missing parameter from body.", StatusCodes.Status400BadRequest);
            }
            else if (updatePasswordRequest.IsNull())
            {
                result = new UserResult(null, "Missing parameters in body.", StatusCodes.Status400BadRequest);
            }
            else
            {
                try
                {
                    var userWithPassword = await _userService.UpdateUserPasswordAsync(updatePasswordRequest);
                    
                    result = new(userWithPassword, "Credentials successfully updated",
                        StatusCodes.Status200OK);
                }
                catch (NotFoundUserInDbException ex)
                {
                    result = new(null, ex.Message,
                        StatusCodes.Status404NotFound);
                }
                catch (IncorrectPasswordException ex)
                {
                    result = new(null, ex.Message,
                        StatusCodes.Status401Unauthorized);
                }
                catch (DatabaseInternalException ex)
                {
                    result = new(null, ex.Message,
                        StatusCodes.Status500InternalServerError);
                }
                catch (Exception ex)
                {
                    result = new(null, $"Internal server error while updating credentials.\n{ex.Message}\n{ex.StackTrace}",
                        StatusCodes.Status500InternalServerError);
                }
            }
            
            return Results.Json(result);
        }
        
        /// <summary>
        /// Checks for authorized user and returns, parses jwt and return info if he exists.
        /// </summary>
        [Authorize]
        [HttpGet("my")]
        [ProducesResponseType(typeof(UserResult), 200)]
        public async Task<IResult> GetUserFromJwt()
        {
            int userId;
            
            UserResult result;
            var id = Request.TryUserGetId();
            
            if (id is null)
            {
                result = new(null, "Missing id.",
                    StatusCodes.Status401Unauthorized);
            }
            else if (!int.TryParse(id, out userId))
            {
                result = new(null, "Unable get id from jwt.",
                    StatusCodes.Status401Unauthorized);
            }
            else
            {
                try
                {
                    var userDto = await _userService.GetUserByIdAsync(userId);
                    
                    result = new(userDto, "There is such user in database", 
                        StatusCodes.Status200OK);
                }
                catch (NotFoundUserInDbException ex)
                {
                    result = new(null, ex.Message,
                        StatusCodes.Status404NotFound);
                }
                catch (Exception ex)
                {
                    result = new(null, $"Internal server error while getting user\n{ex.Message}\n{ex.StackTrace}",
                        StatusCodes.Status500InternalServerError);
                }
            }
            
            return Results.Json(result);
        }
        
        /// <summary>
        /// Updates your password.
        /// </summary>
        [Authorize]
        [HttpPatch("my/password")]
        [ProducesResponseType(typeof(UserResult), 200)]
        public async Task<IResult> UpdateUserPasswordFromJwt([FromBody] MyPasswordUpdateRequest passwordRequest)
        {
            UserResult result;
            
            if (passwordRequest is null)
            {
                result = new UserResult(null, "Missing parameter from body.", StatusCodes.Status400BadRequest);
            }
            else if (passwordRequest.IsNull())
            {
                result = new UserResult(null, "Missing parameters in body.", StatusCodes.Status400BadRequest);
            }
            else
            {
                var id = Request.TryUserGetId();
                int userId;
                if (id is null)
                {
                    result = new(null, "Missing id.",
                        StatusCodes.Status401Unauthorized);
                }
                else if (!int.TryParse(id, out userId))
                {
                    result = new(null, "Unable get id from jwt.",
                        StatusCodes.Status401Unauthorized);
                }
                else
                {
                    try
                    {
                        var userWithPassword = await _userService.UpdateUserPasswordAsync(userId, passwordRequest);
                        result = new(userWithPassword, "Credentials successfully updated",
                            StatusCodes.Status200OK);
                    }
                    catch (NotFoundUserInDbException ex)
                    {
                        result = new(null, ex.Message,
                            StatusCodes.Status404NotFound);
                    }
                    catch (IncorrectPasswordException ex)
                    {
                        result = new(null, ex.Message,
                            StatusCodes.Status401Unauthorized);
                    }
                    catch (DatabaseInternalException ex)
                    {
                        result = new(null, ex.Message,
                            StatusCodes.Status500InternalServerError);
                    }
                    catch (Exception ex)
                    {
                        result = new(null, $"Internal server error while updating credentials.\n{ex.Message}\n{ex.StackTrace}",
                            StatusCodes.Status500InternalServerError);
                    }
                }
            }
            
            return Results.Json(result);
        }
    }
}