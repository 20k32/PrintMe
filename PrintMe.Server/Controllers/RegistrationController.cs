using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Registration;
using PrintMe.Server.Models.Registration;
using PrintMe.Server.Persistence.Registration;

namespace PrintMe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationController : ControllerBase
{
    private IServiceProvider _provider;
    private readonly UserRegistrationLogic _userLogic;
    private readonly UserPersistence _userPersistence;

    public RegistrationController(UserRegistrationLogic userLogic, UserPersistence userPersistence, IServiceProvider provider)
    {
        _userLogic = userLogic;
        _userPersistence = userPersistence;
        _provider = provider;
    }
    /// <summary>
    /// Create a new user and save it to the database.
    /// </summary>
    [HttpPost("registration")]
    public IResult RegisterUser([FromBody] dynamic userData)
    {
        UserRegistrationInfo userInfo =  UserRegistrationLogic.CreateUser(userData.Email, userData.Password, userData.FirstName, userData.LastName);
        _userPersistence.SaveUser(userInfo);
        return Results.Ok(new { message = "User registered successfully" });
    }
}