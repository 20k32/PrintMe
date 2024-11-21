using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Authentication;
using PrintMe.Server.Models.Authentication;
using PrintMe.Server.Persistence;

namespace PrintMe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TestController : ControllerBase
{
    private IServiceProvider _provider;
    public TestController(IServiceProvider provider)
    {
        _provider = provider;
    }
    
    /// <summary>
    /// This may took a lot of time and definitely will take a lot of memory
    /// </summary>
    /// <returns>"Hell to"</returns>
    [HttpGet("getAllUsersFromDb")]
    public IResult GetData()
    {
        var context = _provider.GetService<PrintMeDbContext>();
        var allUsers = context.Users.ToArray();
        var json = Results.Json(allUsers);

        return json;
    }
    
    /// <summary>
    /// <para>This endpoint requires authorization.</para>
    /// Generate token, click 'lock' icon at the right and paste it to get access.
    /// </summary>
    /// <returns></returns>
    [HttpGet("test")]
    [Authorize]
    public IResult AuthorizationTest()
    {
        return Results.Json("Some data");
    }
}