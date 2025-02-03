using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository;

internal sealed class MessageRepository(PrintMeDbContext dbContext)
{
    private readonly PrintMeDbContext _dbContext = dbContext;

    public async Task AddMessageAsync(Message message) => await _dbContext.Messages.AddAsync(message);
}