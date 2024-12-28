using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository;

public class RequestRepository(PrintMeDbContext context)
{
    public async Task<IEnumerable<Request>> GetAllRequestsAsync()
    {
        return await context
            .Requests
            .AsQueryable()
            .ToListAsync();
    }

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

    public async Task<int> GetRequestStatusIdByNameAsync(string status)
    {
      var requestStatus = await context
          .RequestStatuses
          .AsQueryable()
          .FirstOrDefaultAsync(requestStatus => requestStatus.Status == status);

      return requestStatus.RequestStatusId;
    }

    public async Task<IEnumerable<Request>> GetRequestsByStatusIdAsync(int statusId)
    {
        return await context
            .Requests
            .AsQueryable()
            .Where(request => request.RequestStatusId == statusId)
            .ToListAsync();
    }

    public async Task<int> GetRequestStatusReasonIdByNameAsync(string reason)
    {
        var requestStatusReason = await context
            .RequestStatusReasons
            .AsQueryable()
            .FirstOrDefaultAsync(requestStatusReason => requestStatusReason.Reason == reason);

        return requestStatusReason.RequestStatusReasonId;
    }

    public async Task UpdateRequestAsync(Request request)
    {
        var existingEntity = await context.Requests.FindAsync(request.RequestId);
        if (existingEntity != null)
        {
            context.Entry(existingEntity).State = EntityState.Detached;
        }

        context.Requests.Update(request);
        await context.SaveChangesAsync();
    }

    public async Task AddPrinterRequestAsync(Request request)
    {
        await context.Requests.AddAsync(request);
        await context.SaveChangesAsync();
    }

    public async Task EditPrinterRequestAsync(Request request)
    {
        context.Requests.Update(request);
        await context.SaveChangesAsync();
    }

    public async Task<string> GetRequestTypeNameByIdAsync(int requestTypeId)
    {
        var requestType = await context
            .RequestTypes
            .AsQueryable()
            .FirstOrDefaultAsync(requestType => requestType.RequestTypeId == requestTypeId);

        return requestType.Type;
    }

    public async Task AddPrinterRequestMaterialsAsync(int requestId, IEnumerable<int> materialIds)
    {
        var request = await context.Requests.FindAsync(requestId);
        if (request == null)
        {
            return;
        }

        foreach (var materialId in materialIds)
        {
            var material = await context.PrintMaterials1.FindAsync(materialId);
            if (material != null)
            {
                request.PrintMaterials.Add(material);
            }
        }
        
        await context.SaveChangesAsync();
    }
}
