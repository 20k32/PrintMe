using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Models.Api;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs.ChatDto;
using PrintMe.Server.Models.DTOs.UserDto;
using PrintMe.Server.Models.Exceptions;

namespace PrintMe.Server.Controllers;

/// <summary>
/// Controller for managing chats.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ChatController
{
    private UserService _userService;
    private ChatService _chatService;
    private IMapper _mapper;

    public ChatController(IServiceProvider provider)
    {
        _userService = provider.GetRequiredService<UserService>();
        _chatService = provider.GetRequiredService<ChatService>();
        _mapper = provider.GetRequiredService<IMapper>();
    }


    /// <summary>
    /// Creates chat between two users.
    /// </summary>
    [ProducesResponseType(typeof(ApiResult<ChatDto>), 200)]
    [HttpPut("createChat")]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest createChatRequest)
    {
        PlainResult result = null;
        
        if (createChatRequest is null)
        {
            result = new("Missing body.", StatusCodes.Status400BadRequest);
        }
        else if (createChatRequest.IsNull())
        {
            result = new("Missing parameters in body.", StatusCodes.Status400BadRequest);
        }
        else
        {
            try
            {
                var chatDto = _mapper.Map<ChatDto>(createChatRequest);
                await _chatService.AddChatAsync(chatDto);
                result = new ApiResult<ChatDto>(chatDto, "Chat created successfully",
                    StatusCodes.Status200OK);
            }
            catch (NotFoundChatInDbException ex)
            {
                result = new(ex.Message, StatusCodes.Status404NotFound);
            }
            catch (DatabaseInternalException ex)
            {
                result = new(ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                result = new($"Internal server error while updating credentials.\n{ex.Message}\n{ex.StackTrace}",
                    StatusCodes.Status500InternalServerError);
            }
        }

        return result.ToObjectResult();
    }
    
    // <summary>
    /// Adds message to chat.
    /// </summary>
    [ProducesResponseType(typeof(ApiResult<ChatDto>), 200)]
    [HttpPut("addMessage")]
    public async Task<IActionResult> AddMessage([FromBody] AddMessageToChatRequest addMessageRequest)
    {
        PlainResult result = null;
        
        if (addMessageRequest is null)
        {
            result = new("Missing body.", StatusCodes.Status400BadRequest);
        }
        else if (addMessageRequest.IsNull())
        {
            result = new("Missing parameters in body.", StatusCodes.Status400BadRequest);
        }
        else
        {
            try
            {
                await _chatService.AddMessageToChatByIdAsync(addMessageRequest.ChatId,
                    addMessageRequest.Message);
                result = new ApiResult<AddMessageToChatRequest>(addMessageRequest, "Message added to chat successfully",
                    StatusCodes.Status200OK);
            }
            catch (NotFoundChatInDbException ex)
            {
                result = new(ex.Message, StatusCodes.Status404NotFound);
            }
            catch (DatabaseInternalException ex)
            {
                result = new(ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                result = new($"Internal server error while updating credentials.\n{ex.Message}\n{ex.StackTrace}",
                    StatusCodes.Status500InternalServerError);
            }
        }

        return result.ToObjectResult();
    }
}