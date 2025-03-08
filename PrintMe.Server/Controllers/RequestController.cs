using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Constants;
using PrintMe.Server.Logic;
using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Logic.Services.Database.Interfaces;
using PrintMe.Server.Models.Api;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs.RequestDto;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Models.Extensions;
using PrintMe.Server.Models.Filters;

namespace PrintMe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RequestController(IServiceProvider provider) : ControllerBase
{
    private readonly IRequestService _requestService = provider.GetRequiredService<IRequestService>();
    private readonly IUserService _userService = provider.GetRequiredService<IUserService>();

    /// <summary>
    /// Checks for request in database and returns if it is present.
    /// </summary>
    [ProducesResponseType(typeof(ApiResult<IEnumerable<RequestDto>>), 200)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRequest(int id)
    {
        try
        {
            var request = await _requestService.GetRequestByIdAsync(id);
            return Ok(new ApiResult<IEnumerable<RequestDto>>(new List<RequestDto> { request },
                "Request found successfully", StatusCodes.Status200OK));
        }
        catch (NotFoundRequestInDbException ex)
        {
            return NotFound(new PlainResult(ex.Message, StatusCodes.Status404NotFound));
        }
    }

    /// <summary>
    /// Gets all requests for the authenticated user.
    /// </summary>
    [ProducesResponseType(typeof(ApiResult<IEnumerable<RequestDto>>), 200)]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyRequests()
    {
        var id = Request.TryGetUserId();
        if (id is null || !int.TryParse(id, out var userId))
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

            return Ok(new ApiResult<IEnumerable<RequestDto>>(requests, "Requests found successfully",
                StatusCodes.Status200OK));
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new PlainResult($"Internal server error while getting requests: {ex.Message}",
                    StatusCodes.Status500InternalServerError));
        }
    }

    /// <summary>
    /// Gets all requests.
    /// Requests can be filtered by status and type.
    /// Returns all requests if user is admin, else returns only user's requests.
    /// </summary>
    [ProducesResponseType(typeof(ApiResult<IEnumerable<RequestDto>>), 200)]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllRequests([FromQuery] string status, string type)
    {
        try
        {
            IEnumerable<RequestDto> requests = null;
            var userRole = Request.TryGetUserRole();
            var userid = Request.TryGetUserId();
            if (userid == null || !int.TryParse(userid, out var userId))
            {
                return BadRequest(new PlainResult("Unable to get user id from token", StatusCodes.Status400BadRequest));
            }
            if (userRole == null) 
            {
                return BadRequest(new PlainResult("Unable to get user role from token", StatusCodes.Status400BadRequest));
            }
            var filter = new RequestFilter { Status = status, Type = type };
            
            if (userRole != DbConstants.UserRole.Admin)
            { 
                requests = (await _requestService.GetRequestsByUserIdAsync(userId, filter)).ToList();
            }
            else if (userRole == DbConstants.UserRole.Admin)
            { 
                requests = await _requestService.GetAllRequestsAsync(filter);
            }
            return Ok(new ApiResult<IEnumerable<RequestDto>>(requests, "Requests found successfully", StatusCodes.Status200OK));
        }
        catch (NotFoundRequestInDbException ex)
        {
            return NotFound(new PlainResult(ex.Message, StatusCodes.Status404NotFound));
        }
        catch (NotFoundRequestStatusInDb ex)
        {
            return NotFound(new PlainResult(ex.Message, StatusCodes.Status404NotFound));
        }
        catch (NotFoundRequestTypeInDb ex)
        {
            return NotFound(new PlainResult(ex.Message, StatusCodes.Status404NotFound));
        }
    }

    /// <summary>
    /// Approves a request.
    /// </summary>
    /// <param name="id">Request id.</param>
    [ProducesResponseType(typeof(ApiResult<IEnumerable<RequestDto>>), 200)]
    [HttpPost("{id:int}/approve")]
    public async Task<IActionResult> ApproveRequest(int id)
    {
        try
        {
            var request = await _requestService.GetRequestByIdAsync(id);

            try
            {
                await _requestService.ApproveRequestAsync(request, provider);
            }
            catch (NotFoundStrategyException ex)
            {
                return BadRequest(new PlainResult(ex.Message, StatusCodes.Status400BadRequest));
            }
            catch (AlreadyApprovedRequestException ex)
            {
                return BadRequest(new PlainResult(ex.Message, StatusCodes.Status400BadRequest));
            }
            catch (NotFoundRequestTypeInDb ex)
            {
                return BadRequest(new PlainResult(ex.Message, StatusCodes.Status400BadRequest));
            }


            return Ok(new PlainResult("Request approved successfully", StatusCodes.Status200OK));
        }
        catch (NotFoundRequestInDbException ex)
        {
            return NotFound(new PlainResult(ex.Message, StatusCodes.Status404NotFound));
        }
        catch (NotFoundRequestStatusInDb ex)
        {
            return BadRequest(new PlainResult(ex.Message, StatusCodes.Status400BadRequest));
        }
    }

    /// <summary>
    /// Declines a request.
    /// </summary>
    /// <param name="id">Request id.</param>
    /// <param name="reason">Decline reason.</param>
    [ProducesResponseType(typeof(ApiResult<IEnumerable<RequestDto>>), 200)]
    [HttpPost("{id:int}/decline")]
    public async Task<IActionResult> DeclineRequest(int id, string reason)
    {
        try
        {
            var request = await _requestService.GetRequestByIdAsync(id);

            if (string.IsNullOrEmpty(reason))
            {
                reason = "NO_REASON";
            }

            await _requestService.DeclineRequestAsync(request, reason);

            return Ok(new PlainResult("Request declined successfully",
                StatusCodes.Status200OK));
        }
        catch (AlreadyApprovedRequestException ex)
        {
            return BadRequest(new PlainResult(ex.Message, StatusCodes.Status400BadRequest));
        }
        catch (NotFoundRequestInDbException ex)
        {
            return NotFound(new PlainResult(ex.Message, StatusCodes.Status404NotFound));
        }
        catch (NotFoundRequestStatusInDb ex)
        {
            return NotFound(new PlainResult(ex.Message, StatusCodes.Status404NotFound));
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
        if (id is null || !int.TryParse(id, out var userId))
        {
            return Unauthorized(new PlainResult("Unable to get user id from token", StatusCodes.Status401Unauthorized));
        }

        request.UserId = userId;

        try
        {
            var isUserEmailVerified = _userService.GetUserByIdAsync(userId).Result.isVerified;
            if (isUserEmailVerified == false)
            {
                throw new EmailNotVerifiedException();
            }

            await _requestService.AddPrinterRequestAsync(request);
        }
        catch (EmailNotVerifiedException ex)
        {
            return StatusCode(403,
                new PlainResult(ex.Message, StatusCodes.Status403Forbidden));
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new PlainResult($"Internal server error while adding request: {ex.Message}",
                    StatusCodes.Status500InternalServerError));
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
        if (id is null || !int.TryParse(id, out var userId))
        {
            return Unauthorized(new PlainResult("Unable to get user id from token", StatusCodes.Status401Unauthorized));
        }

        try
        {
            var isUserEmailVerified = _userService.GetUserByIdAsync(userId).Result.isVerified;
            if (isUserEmailVerified == false)
            {
                throw new EmailNotVerifiedException();
            }

            await _requestService.EditPrinterRequestAsync(request, userId);
        }
        catch (EmailNotVerifiedException ex)
        {
            return StatusCode(403,
                new PlainResult(ex.Message, StatusCodes.Status403Forbidden));
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new PlainResult($"Internal server error while editing request: {ex.Message}",
                    StatusCodes.Status500InternalServerError));
        }

        return Ok(new PlainResult("Request edited successfully", StatusCodes.Status200OK));
    }
}
