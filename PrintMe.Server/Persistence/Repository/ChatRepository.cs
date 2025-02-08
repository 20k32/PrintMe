using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository;

internal sealed class ChatRepository(PrintMeDbContext dbContext)
{
    private readonly PrintMeDbContext _dbContext = dbContext;

    public async Task<Chat> GetChatByIdAsync(int chatId) =>
        await _dbContext.Chats.FirstOrDefaultAsync(existing => existing.ChatId == chatId);

    /// <summary>
    /// Get chat by user ids, order matters
    /// </summary>
    /// <param name="user1Id"></param>
    /// <param name="user2Id"></param>
    /// <returns></returns>
    public async Task<Chat> GetByUsersIdsStrictAsync(int user1Id, int user2Id) =>
        await _dbContext.Chats.FirstOrDefaultAsync(existing => existing.User1.UserId == user1Id
                                                                && existing.User2.UserId == user2Id);

    /// <summary>
    /// Get chat by user ids, order of ids does not matter
    /// </summary>
    /// <param name="user1Id"></param>
    /// <param name="user2Id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<Chat> GetByUserIdsInexactAsync(int user1Id, int user2Id)
    {
        var result = await GetByUsersIdsStrictAsync(user1Id, user2Id);
        
        if (result is null)
        {
            result = await GetByUsersIdsStrictAsync(user2Id, user1Id);

            if (result is null)
            {
                throw new ArgumentNullException(nameof(result));
            }
        }

        return result;
    }
    
    public async Task<Chat> AddMessageToChatByIdAsync(int chatId, Message message)
    {
        var chat = await GetChatByIdAsync(chatId);

        if (chat is null)
        {
            throw new NotFoundChatInDbException();
        }
            
        chat.Messages.Add(message);
            
        _dbContext.Chats.Update(chat);
        
        await _dbContext.SaveChangesAsync();

        return chat;
        
    }

    public async Task<Chat> AddChatAsync(Chat chat)
    {
        await _dbContext.Chats.AddAsync(chat);
        await _dbContext.SaveChangesAsync();

        return chat;
    }

    public async Task<Chat> UpdateChatByIdAsync(int chatId, Chat chat)
    {
        var existingChat = await GetChatByIdAsync(chatId);

        if (existingChat is not null)
        {
            existingChat.ChatId = chat.ChatId;
            existingChat.User1 = chat.User1;
            existingChat.User2 = chat.User2;
            existingChat.IsArchived = chat.IsArchived;

            _dbContext.Chats.Update(existingChat);
            await _dbContext.SaveChangesAsync();

            return existingChat;
        }

        throw new ArgumentNullException(nameof(existingChat));
    }

    public async Task<ICollection<Chat>> GetChatsByUserIdAsync(int userId) => 
        await _dbContext.Chats.Where(existing =>
        existing.User1.UserId == userId || existing.User2.UserId == userId).ToListAsync();

    public async Task<ICollection<Message>> GetMessagesByChatIdAsync(int chatId)
    {
        var chat = await GetChatByIdAsync(chatId);

        if (chat is not null)
        {
            return chat.Messages;
        }

        throw new ArgumentNullException(nameof(chat));
    }
}