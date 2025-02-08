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
using PrintMe.Server.Models.Extensions;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Controllers;

/// <summary>
/// Controller for managing chats.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
public class ChatController : ControllerBase
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
    
    private async Task<PlainResult> CreateChatCoreAsync(ChatDto chatDto)
    {
        PlainResult result = null;
        
        try
        {
            var chatResult = await _chatService.AddChatAsync(chatDto);
            result = new ApiResult<ChatDto>(chatResult, "Chat created successfully",
                StatusCodes.Status200OK);
        }
        catch (IncorrectChatParametersException ex)
        {
            result = new(ex.Message, StatusCodes.Status405MethodNotAllowed);
        }
        catch (FoundChatInDbException ex)
        {
            result = new(ex.Message, StatusCodes.Status302Found);
        }
        catch (NotFoundChatInDbException ex)
        {
            result = new(ex.Message, StatusCodes.Status404NotFound);
        }
        catch (NotFoundUserInDbException ex)
        {
            result = new(ex.Message,
                StatusCodes.Status404NotFound);
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

        return result;
    }

    private async Task<PlainResult> AddMessageCoreAsync(int chatId, MessageDto message)
    {
        PlainResult result = null;
        
        try
        {
            await _chatService.AddMessageToChatByIdAsync(chatId,
                message);
            result = new ApiResult<MessageDto>(message, "Message added to chat successfully",
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

        return result;
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
            var chatDto = _mapper.Map<ChatDto>(createChatRequest);

            result = await CreateChatCoreAsync(chatDto);
        }

        return result.ToObjectResult();
    }
    
    /// <summary>
    /// Creates chat between two users.
    /// </summary>
    [ProducesResponseType(typeof(ApiResult<ChatDto>), 200)]
    [HttpPut("createChatForMe")]
    public async Task<IActionResult> CreateChatForMe([FromBody] CreateChatForMeRequest createChatRequest)
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
            var user1IdRaw = Request.TryGetUserId();

            if (user1IdRaw is null || !int.TryParse(user1IdRaw, out var user1Id))
            {
                return Unauthorized(new PlainResult("Unable to get user id from token", StatusCodes.Status401Unauthorized));
            }
                
            var chatDto = new ChatDto(default, user1Id, createChatRequest.User2Id, createChatRequest.ShouldArchive);
            result = await CreateChatCoreAsync(chatDto);
        }

        return result.ToObjectResult();
    }
    
    // <summary>
    /// Adds message to chat from specific user.
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
            result = await AddMessageCoreAsync(addMessageRequest.ChatId, addMessageRequest.Message);
        }

        return result.ToObjectResult();
    }
    
    // <summary>
    /// Adds message to chat from sender (sender's id, from jwt).
    /// </summary>
    [ProducesResponseType(typeof(ApiResult<ChatDto>), 200)]
    [HttpPut("sendMessage")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageToChatRequest sendMessage)
    {
        PlainResult result = null;
        
        if (sendMessage is null)
        {
            result = new("Missing body.", StatusCodes.Status400BadRequest);
        }
        else if (sendMessage.IsNull())
        {
            result = new("Missing parameters in body.", StatusCodes.Status400BadRequest);
        }
        else
        {
            try
            {
                var senderId = Request.TryGetUserId();

                if (senderId is null || !int.TryParse(senderId, out var user1Id))
                {
                    return Unauthorized(new PlainResult("Unable to get user id from token", StatusCodes.Status401Unauthorized));
                }

                var messageDto = new MessageDto(sendMessage.ChatId, senderId, sendMessage.SendedDateTime, sendMessage.Payload);

                result = await AddMessageCoreAsync(sendMessage.ChatId, messageDto);
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