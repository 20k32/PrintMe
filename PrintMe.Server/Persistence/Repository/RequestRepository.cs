using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository;

public class RequestRepository(PrintMeDbContext context)
{
    public async Task<object> GetRequestByIdAsync(int requestId)
    {
        return await context
            .Requests
            .AsQueryable()
            .FirstOrDefaultAsync(request => request.RequestId == requestId);
    }

    internal async Task<IEnumerable<Request>> GetRequestsByUserIdAsync(int userId)
    {
        return await context
            .Requests
            .AsQueryable()
            .Where(request => request.UserSenderId == userId)
            .ToListAsync();
    }
    
    public async Task AddPrinterAsync(Request request)
    {
        await context.Requests.AddAsync(request);
        await context.SaveChangesAsync();
    }
}