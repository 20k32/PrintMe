using Microsoft.AspNetCore.Mvc;

namespace PrintMe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TestController : ControllerBase
{
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
    /// Get numbers hahahah
    /// </summary>
    /// <returns>123123123</returns>
    [HttpGet("TestGetNumbers")]
    public IResult GetNumbers()
    {
        var dataObject =
            new {
                Value = "123123123"
            };

        var json = Results.Json(dataObject);

        return json;
    }
}