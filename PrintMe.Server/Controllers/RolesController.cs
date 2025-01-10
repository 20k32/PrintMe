using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic;
using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Models.Api;
using PrintMe.Server.Models.Authentication;
using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Models.Extensions;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController(IServiceProvider provider) : ControllerBase
    {
        [HttpGet("my")]
        public IActionResult GetMyRole()
        {
            PlainResult result;
            
            try
            {
                var userId = Request.TryGetUserId();

                var userService = provider.GetService<UserService>();

                var userRole = userService.GetUserRole(int.Parse(userId));
                    
                if (string.IsNullOrWhiteSpace(userId))
                {
                    result = new ("Missing parameters in JWT.", StatusCodes.Status400BadRequest);
                }
                else if (!Roles.IsRolePresent(userRole))
                {
                    result = new("Incorrect role.",
                        StatusCodes.Status401Unauthorized);
                }
                else
                {
                    result = new ApiResult<RoleEntity>(new() { UserRole = userRole},
                        "Role is present.",
                        StatusCodes.Status200OK);
                }
            }
            catch (Exception ex)
            {
                result = new($"Internal server error while getting role.\n{ex.Message}\n{ex.StackTrace}",
                    StatusCodes.Status500InternalServerError);
            }
            
            return result.ToObjectResult();
        }
    }
}