using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository.Interfaces;

public interface IMessageRepository
{
    Task AddMessageAsync(Message message);
    Task<ICollection<Message>> GetMessagesAsync(int chatId);
}