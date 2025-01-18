using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Constants;
using PrintMe.Server.Logic;
using PrintMe.Server.Logic.Authentication;
using PrintMe.Server.Logic.Helpers;
using PrintMe.Server.Models.Authentication;
using PrintMe.Server.Models.DTOs.UserDto;
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
        
        var dtos = new List<PasswordUserDto>();
        
        foreach (var user in allUsers)
        {
            dtos.Add(user.MapToPasswordUserDto());
        }
        
        var json = Results.Json(dtos);
        return json;
    }
    
    [HttpGet("generateTestData")]
    public async Task<IResult> LoadData()
    {
        var context = _provider.GetService<PrintMeDbContext>();
        await context.LoadTestDataAsync();

        return Results.Text("Data generated (or not)");
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
    
    [HttpGet("GenerateJwt")]
    public IResult GenerateToken()
    {
        var loginResult = new SuccessLoginEntity(1, string.Empty, DbConstants.UserRole.User);
        var tokenGenerator = _provider.GetService<TokenGenerator>();
        var token = tokenGenerator.GetForSuccessLoginResult(loginResult);
        return Results.Json(token);
    }
}