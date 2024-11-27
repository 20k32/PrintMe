using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Models.Api.ApiResult.Common;
using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Models.Extensions;

namespace PrintMe.Server.Controllers;

[ApiController]
[Route("api/request")]
public class RequestController(IServiceProvider provider) : ControllerBase
{
    private readonly RequestService _requestService = provider.GetRequiredService<RequestService>();

    /// <summary>
    /// Checks for request in database and returns if it is present.
    /// </summary>
    [ProducesResponseType(typeof(RequestResult), 200)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRequest(int id)
    {
        try
        {
            var request = await _requestService.GetRequestByIdAsync(id);
            return Ok(new RequestResult(new List<RequestDto> { request }, "Request found successfully", StatusCodes.Status200OK));
        }
        catch (NotFoundRequestInDbException)
        {
            return NotFound(new RequestResult(null, "Request not found", StatusCodes.Status404NotFound));
        }
    }

    /// <summary>
    /// Gets all requests for the authenticated user.
    /// </summary>
    [Authorize]
    [ProducesResponseType(typeof(RequestResult), 200)]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyRequests()
    {
        var id = Request.TryUserGetId();
        if (id is null || !int.TryParse(id, out int userId))
        {
            return Unauthorized(new RequestResult(null, "Unable to get user id from token", StatusCodes.Status401Unauthorized));
        }

        try
        {
            var requests = (await _requestService.GetRequestsByUserIdAsync(userId)).ToList();
            if (requests.Count == 0)
            {
                return NotFound(new RequestResult(null, "No requests found", StatusCodes.Status404NotFound));
            }
            return Ok(new RequestResult(requests, "Requests found successfully", StatusCodes.Status200OK));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new RequestResult(null, $"Internal server error while getting requests: {ex.Message}", StatusCodes.Status500InternalServerError));
        }
    }
}