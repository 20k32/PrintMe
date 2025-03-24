using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Models.DTOs.ChatDto;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Persistence;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository;

namespace PrintMe.Server.Logic.Services.Database;

internal sealed class ChatService(MessageRepository messageRepository, ChatRepository chatRepository, UserService userService, IMapper mapper)
{
    private readonly MessageRepository _messageRepository = messageRepository;
    private readonly ChatRepository _chatRepository = chatRepository;
    private readonly UserService _userService = userService;
    private readonly IMapper _mapper = mapper;

    public async Task<ChatDto> GetChatByIdAsync(int chatId)
    {
        try
        {
            var existingChat = await _chatRepository.GetChatByIdAsync(chatId);
            
            if (existingChat is null)
            {
                throw new NotFoundChatInDbException();
            }
            
            return _mapper.Map<ChatDto>(existingChat);
        }
        catch(Exception ex)
        {
            throw new DatabaseInternalException(ex);
        }
    }

    public async Task<ChatDto> GetChatByUsersIdsStrictAsync(int user1Id, int user2Id)
    {
        try
        {
            var result = await _chatRepository.GetByUsersIdsStrictAsync(user1Id, user2Id);

            if (result is null)
            {
                throw new NotFoundChatInDbException();
            }

            return _mapper.Map<ChatDto>(result);
        }
        catch(ArgumentNullException ex)
        {
            throw new DatabaseInternalException(ex);
        }
    }

    public async Task<ChatDto> GetChatByUserIdsAsyncInexact(int user1Id, int user2Id)
    {
        try
        {
            var result = await _chatRepository.GetByUserIdsInexactAsync(user1Id, user2Id);

            if (result is null)
            {
                throw new NotFoundChatInDbException();
            }

            return _mapper.Map<ChatDto>(result);
        }
        catch(ArgumentNullException ex)
        {
            throw new DatabaseInternalException(ex);
        }
    }
    
    public async Task<ChatDto> AddMessageToChatByIdAsync(int chatId, MessageDto message)
    {
        var messageRaw = _mapper.Map<Message>(message);

        var sender = await _userService.GetUserByIdAsync(messageRaw.SenderId);

        if (sender is null)
        {
            throw new NotFoundUserInDbException();
        }

        var chat = await _chatRepository.GetChatByIdAsync(chatId);

        if (chat is null 
            || chat.User1Id != sender.UserId 
            && chat.User2Id != sender.UserId)
        {
            throw new NotFoundChatInDbException();
        }

        await _messageRepository.AddMessageAsync(messageRaw);
        
        var result = await _chatRepository.AddMessageToChatByIdAsync(chatId, messageRaw);

        return _mapper.Map<ChatDto>(result);
    }

    public async Task<ChatDto> AddChatAsync(ChatDto chat)
    {
        var firstUser = await _userService.GetUserByIdAsync(chat.User1Id);
        var secondUser = await _userService.GetUserByIdAsync(chat.User2Id);

        if (chat.User1Id == chat.User2Id)
        {
            throw new IncorrectChatParametersException();
        }
        
        if (firstUser is null || secondUser is null)
        {
            throw new NotFoundUserInDbException();
        }

        Chat existingChat = null;

        try
        {
            existingChat = await _chatRepository.GetByUserIdsInexactAsync(chat.User1Id, chat.User2Id);
        }
        catch (ArgumentNullException)
        {
            existingChat = null;
        }
        
        if (existingChat is not null)
        {
            throw new FoundChatInDbException();
        }
        
        var chatRaw = _mapper.Map<Chat>(chat);
        
       var result = await _chatRepository.AddChatAsync(chatRaw);

       if (result is null)
       {
           throw new DatabaseInternalException();
       }

       return _mapper.Map<ChatDto>(result);
    }

    public async Task<ChatDto> UpdateChatByIdAsync(int chatId, ChatDto chat)
    {
        var existingChat = await _chatRepository.GetChatByIdAsync(chatId);
        
        if (existingChat is not null)
        {
            existingChat.User1Id = chat.User1Id;
            existingChat.User2Id = chat.User2Id;
            
            var user1Dto = await _userService.GetUserByIdAsync(chat.User1Id);
            var user2Dto = await _userService.GetUserByIdAsync(chat.User2Id);
            
            var user1Raw = _mapper.Map<User>(user1Dto);
            var user2Raw = _mapper.Map<User>(user2Dto);
            
            existingChat.User1 = user1Raw; 
            existingChat.User2 = user2Raw;
            
            existingChat.IsArchived = chat.IsArchived;

            existingChat =
                await _chatRepository.UpdateChatByIdAsync(existingChat.ChatId, existingChat);

            return _mapper.Map<ChatDto>(existingChat);
        }

        throw new NotFoundChatInDbException();
    }

    public async Task<ICollection<ChatDto>> GetChatsByUserIdAsync(int userId)
    {
        var chatsRaw = await _chatRepository.GetChatsByUserIdAsync(userId);

        if (chatsRaw is null)
        {
            throw new NotFoundUserInDbException();
        }

        return _mapper.Map<ICollection<ChatDto>>(chatsRaw);
    }
    
    public async Task<ICollection<MessageDto>> GetMessagesByChatIdAsync(int chatId)
    {
        var messages = await _chatRepository.GetMessagesByChatIdAsync(chatId);

        if (messages is null)
        {
            throw new NotFoundChatInDbException();
        }

        return _mapper.Map<ICollection<MessageDto>>(messages);
    }
    
    public async Task<ICollection<MessageDto>> GetMessagesByChatIdAsync(int user1Id, int user2Id, int chatId)
    {
        var messages = await _chatRepository.GetMessagesByChatIdAsync(user1Id, user2Id, chatId);

        if (messages is null)
        {
            throw new NotFoundChatInDbException();
        }

        return _mapper.Map<ICollection<MessageDto>>(messages);
    }

    public async Task<ChatDto> ArchiveChatAsync(int chatId, int senderId)
    {
        var existingChat = await _chatRepository.GetChatByIdAsync(chatId);

        if (existingChat is null || existingChat.User1Id != senderId && existingChat.User2Id != senderId)
        {
            throw new NotFoundChatInDbException();
        }

        if (existingChat.IsArchived is true)
        {
            throw new ChatAlreadyArchivedException();
        }
        
        existingChat.IsArchived = true;

        var chatRaw = _mapper.Map<Chat>(existingChat);

        var updateResult = await _chatRepository.UpdateChatAsync(chatRaw);

        if (updateResult is null)
        {
            throw new DatabaseInternalException();
        }
        
        return _mapper.Map<ChatDto>(updateResult);
    }
}

