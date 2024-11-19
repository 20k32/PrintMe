using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Models.ApiRequest;
using PrintMe.Server.Models.ApiResult;
using PrintMe.Server.Models.ApiResult.Auth;
using PrintMe.Server.Models.ApiResult.Common;
using PrintMe.Server.Persistence;

namespace PrintMe.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController
    {
        private IServiceProvider _provider;
        
        public UserController(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        [HttpGet("getUserByEmail")]
        public IResult GetUserByEmail(UserByEmailRequest user)
        {
            UserResult result = null;
            var context = _provider.GetService<PrintMeDbContext>();
            
            var dbUser = context.Users
                .AsQueryable()
                .FirstOrDefault(existing => existing.Email.Equals(user.Email));

            return null;
        }
    }
}