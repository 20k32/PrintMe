using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository.Interfaces;

namespace PrintMe.Server.Persistence.Repository;

internal sealed class MessageRepository(PrintMeDbContext dbContext) : IMessageRepository
{
    private readonly PrintMeDbContext _dbContext = dbContext;

    public async Task AddMessageAsync(Message message)
    {
        await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<ICollection<Message>> GetMessagesAsync(int chatId)
        => await _dbContext.Messages.Where(message => message.ChatId == chatId).ToListAsync();
}