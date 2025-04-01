using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository.Interfaces;

public interface IChatRepository
{
    Task<Chat> GetChatByIdAsync(int chatId);
    Task<Chat> GetChatWithMessagesByIdAsync(int chatId);
    Task<Chat> UpdateChatAsync(Chat chat);
    Task<Chat> GetByUsersIdsStrictAsync(int user1Id, int user2Id);
    Task<Chat> GetByUserIdsInexactAsync(int user1Id, int user2Id);
    Task<Chat> AddMessageToChatByIdAsync(int chatId, Message message);
    Task<Chat> AddChatAsync(Chat chat);
    Task<Chat> UpdateChatByIdAsync(int chatId, Chat chat);
    Task<ICollection<Chat>> GetChatsByUserIdAsync(int userId);
    Task<ICollection<Message>> GetMessagesByChatIdAsync(int chatId);
    Task<ICollection<Message>> GetMessagesByChatIdAsync(int user1Id, int user2Id, int chatId);
}