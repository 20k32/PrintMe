using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Models.Api;
using PrintMe.Server.Models.Api.ApiRequest;
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
    [ProducesResponseType(typeof(ApiResult<IEnumerable<RequestDto>>), 200)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRequest(int id)
    {
        try
        {
            var request = await _requestService.GetRequestByIdAsync(id);
            return Ok(new ApiResult<IEnumerable<RequestDto>>(new List<RequestDto> { request }, "Request found successfully", StatusCodes.Status200OK));
        }
        catch (NotFoundRequestInDbException)
        {
            return NotFound(new PlainResult("Request not found", StatusCodes.Status404NotFound));
        }
    }

    /// <summary>
    /// Gets all requests for the authenticated user.
    /// </summary>
    [Authorize]
    [ProducesResponseType(typeof(ApiResult<IEnumerable<RequestDto>>), 200)]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyRequests()
    {
        var id = Request.TryGetUserId();
        if (id is null || !int.TryParse(id, out int userId))
        {
            return Unauthorized(new PlainResult("Unable to get user id from token", StatusCodes.Status401Unauthorized));
        }

        try
        {
            var requests = (await _requestService.GetRequestsByUserIdAsync(userId)).ToList();
            if (requests.Count == 0)
            {
                return NotFound(new PlainResult("No requests found", StatusCodes.Status404NotFound));
            }
            return Ok(new PlainResult( "Requests found successfully", StatusCodes.Status200OK));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new PlainResult($"Internal server error while getting requests: {ex.Message}", StatusCodes.Status500InternalServerError));
        }
    }
    
    /// <summary>
    /// Request for adding new printer to the database.
    /// </summary>
    [Authorize]
    [ProducesResponseType(typeof(ApiResult<RequestDto>), 200)]
    [HttpPost("add")]
    public async Task<IActionResult> AddPrinterRequest([FromBody] AddPrinterRequest request)
    {
        var id = Request.TryGetUserId();
        if (id is null || !int.TryParse(id, out int userId))
        {
            return Unauthorized(new PlainResult("Unable to get user id from token", StatusCodes.Status401Unauthorized));
        }
        try
        {
            await _requestService.AddPrinterRequestAsync(request, userId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new PlainResult($"Internal server error while adding request: {ex.Message}", StatusCodes.Status500InternalServerError));
        }
        return Ok(new PlainResult("Request added successfully", StatusCodes.Status200OK));
    }
    
    /// <summary>
    /// Request for editing printer in the database. 
    /// </summary>
    
    [Authorize]
    [ProducesResponseType(typeof(ApiResult<RequestDto>), 200)]
    [HttpPost("edit")]
    public async Task<IActionResult> EditPrinterRequest([FromBody] EditPrinterRequest request)
    {
        var id = Request.TryGetUserId();
        if (id is null || !int.TryParse(id, out int userId))
        {
            return Unauthorized(new PlainResult("Unable to get user id from token", StatusCodes.Status401Unauthorized));
        }
        try
        {
            await _requestService.EditPrinterRequestAsync(request, userId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new PlainResult($"Internal server error while editing request: {ex.Message}", StatusCodes.Status500InternalServerError));
        }
        return Ok(new PlainResult("Request edited successfully", StatusCodes.Status200OK));
    }
}