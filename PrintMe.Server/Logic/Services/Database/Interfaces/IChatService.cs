using PrintMe.Server.Models.DTOs.ChatDto;

namespace PrintMe.Server.Logic.Services.Database.Interfaces;

public interface IChatService
{
    Task<ChatDto> GetChatByIdAsync(int chatId);
    Task<ChatDto> GetChatByUsersIdsStrictAsync(int user1Id, int user2Id);
    Task<ChatDto> GetChatByUserIdsAsyncInexact(int user1Id, int user2Id);
    Task<ChatDto> AddMessageToChatByIdAsync(int chatId, MessageDto message);
    Task<ChatDto> AddChatAsync(ChatDto chat);
    Task<ChatDto> UpdateChatByIdAsync(int chatId, ChatDto chat);
    Task<ICollection<ChatDto>> GetChatsByUserIdAsync(int userId);
    Task<ICollection<MessageDto>> GetMessagesByChatIdAsync(int chatId);
    Task<ICollection<MessageDto>> GetMessagesByChatIdAsync(int user1Id, int user2Id, int chatId);
    Task<ChatDto> ArchiveChatAsync(int chatId, int senderId);
    
}