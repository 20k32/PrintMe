using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Authentication;
using PrintMe.Server.Models.Authentication;

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
    /// Get data "Hell to world"
    /// </summary>
    /// <returns>"Hell to"</returns>
    [HttpGet("TestGetData")]
    public IResult GetData()
    {
        var dataObject =
        new {
            Value = "Hell to"
        };

        var json = Results.Json(dataObject);

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