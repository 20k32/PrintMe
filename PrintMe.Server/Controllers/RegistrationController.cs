using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Registration;

namespace PrintMe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationController : ControllerBase
{
    private IServiceProvider _provider;
   // private readonly UserPersistence _userPersistence;

    public RegistrationController(IServiceProvider provider)
    {
       // _userPersistence = provider.GetService<UserPersistence>();
        _provider = provider;
    }
    /// <summary>
    /// Create a new user and save it to the database.
    /// </summary>
    [HttpPost("registration")]
    public IResult RegisterUser([FromBody] dynamic userData)
    {
        //User userInfo =  UserRegistrationLogic.CreateUser(userData.Email, userData.Password, userData.FirstName, userData.LastName);
       // _userPersistence.SaveUser(userInfo);
        return Results.Ok(new { message = "User registered successfully" });
    }
}