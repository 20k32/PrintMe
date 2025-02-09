using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository;

internal sealed class MessageRepository(PrintMeDbContext dbContext)
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