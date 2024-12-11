using Bogus.Extensions.Italy;
using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Models;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository;

public class RequestRepository(PrintMeDbContext context)
{
    public async Task<IEnumerable<Request>> GetAllRequestsAsync()
    {
        return await context
            .Requests
            .AsQueryable()
            .Include(request => request.RequestStatus)
            .Include(request => request.RequestType)
            .Select(request => new Request()
            {
                Description = request.Description,
                DeleteUser = request.DeleteUser,
                LocationX = request.LocationX,
                LocationY = request.LocationY,
                ModelId = request.ModelId,
                PrintMaterials = request.PrintMaterials,
                ReportedUser = request.ReportedUser,
                RequestId = request.RequestId,
                RequestStatus = new ()
                {
                    Status = request.RequestStatus.Status,
                    RequestStatusId = request.RequestStatusId,
                    Requests = null // exclude third table
                },
                RequestType = new()
                {
                    RequestTypeId = request.RequestType.RequestTypeId,
                    Type = request.RequestType.Type,
                },
                UserSender = request.UserSender,
                DeleteUserId = request.DeleteUserId,
                MaxModelHeight = request.MaxModelHeight,
                MaxModelWidth = request.MaxModelWidth,
                MinModelHeight = request.MinModelHeight,
                MinModelWidth = request.MinModelWidth,
                ReportedUserId = request.ReportedUserId,
                RequestStatusId = request.RequestStatusId,
                RequestStatusReason = request.RequestStatusReason,
                RequestTypeId = request.RequestTypeId,
                UserSenderId = request.UserSenderId,
                UserTextData = request.UserTextData,
                RequestStatusReasonId = request.RequestStatusReasonId
            })
            .ToListAsync();
    }

    public async Task<Request> GetRequestByIdAsync(int requestId)
    {
        return await context
            .Requests
            .AsQueryable()
            .Include(request => request.RequestStatus)
            .Include(request => request.RequestType)
            .Select(request => new Request()
            {
                Description = request.Description,
                DeleteUser = request.DeleteUser,
                LocationX = request.LocationX,
                LocationY = request.LocationY,
                ModelId = request.ModelId,
                PrintMaterials = request.PrintMaterials,
                ReportedUser = request.ReportedUser,
                RequestId = request.RequestId,
                RequestStatus = new ()
                {
                    Status = request.RequestStatus.Status,
                    RequestStatusId = request.RequestStatusId,
                    Requests = null // exclude third table
                },
                RequestType = new()
                {
                    RequestTypeId = request.RequestType.RequestTypeId,
                    Type = request.RequestType.Type,
                },
                UserSender = request.UserSender,
                DeleteUserId = request.DeleteUserId,
                MaxModelHeight = request.MaxModelHeight,
                MaxModelWidth = request.MaxModelWidth,
                MinModelHeight = request.MinModelHeight,
                MinModelWidth = request.MinModelWidth,
                ReportedUserId = request.ReportedUserId,
                RequestStatusId = request.RequestStatusId,
                RequestStatusReason = request.RequestStatusReason,
                RequestTypeId = request.RequestTypeId,
                UserSenderId = request.UserSenderId,
                UserTextData = request.UserTextData,
                RequestStatusReasonId = request.RequestStatusReasonId
            })
            .FirstOrDefaultAsync(request => request.RequestId == requestId);
    }

    internal async Task<IEnumerable<Request>> GetRequestsByUserIdAsync(int userId)
    {
        return await context
            .Requests
            .AsQueryable()
            .Where(request => request.UserSenderId == userId)
            .Include(request => request.RequestStatus)
            .Include(request => request.RequestType)
            .Select(request => new Request()
            {
                Description = request.Description,
                DeleteUser = request.DeleteUser,
                LocationX = request.LocationX,
                LocationY = request.LocationY,
                ModelId = request.ModelId,
                PrintMaterials = request.PrintMaterials,
                ReportedUser = request.ReportedUser,
                RequestId = request.RequestId,
                RequestStatus = new ()
                {
                    Status = request.RequestStatus.Status,
                    RequestStatusId = request.RequestStatusId,
                    Requests = null
                },
                RequestType = new()
                {
                    RequestTypeId = request.RequestType.RequestTypeId,
                    Type = request.RequestType.Type,
                },
                UserSender = request.UserSender,
                DeleteUserId = request.DeleteUserId,
                MaxModelHeight = request.MaxModelHeight,
                MaxModelWidth = request.MaxModelWidth,
                MinModelHeight = request.MinModelHeight,
                MinModelWidth = request.MinModelWidth,
                ReportedUserId = request.ReportedUserId,
                RequestStatusId = request.RequestStatusId,
                RequestStatusReason = request.RequestStatusReason,
                RequestTypeId = request.RequestTypeId,
                UserSenderId = request.UserSenderId,
                UserTextData = request.UserTextData,
                RequestStatusReasonId = request.RequestStatusReasonId
            })
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
    
    public async Task<RequestStatus> GetRequestStatusByNameAsync(string status)
    {
        var requestStatus = await context
            .RequestStatuses
            .AsQueryable()
            .FirstOrDefaultAsync(requestStatus => requestStatus.Status == status);

        return requestStatus;
    }

    public async Task<IEnumerable<Request>> GetRequestsByStatusIdAsync(int statusId)
    {
        return await context
            .Requests
            .AsQueryable()
            .Where(request => request.RequestStatusId == statusId)
            .Include(request => request.RequestStatus)
            .Include(request => request.RequestType)
            .Select(request => new Request()
            {
                Description = request.Description,
                DeleteUser = request.DeleteUser,
                LocationX = request.LocationX,
                LocationY = request.LocationY,
                ModelId = request.ModelId,
                PrintMaterials = request.PrintMaterials,
                ReportedUser = request.ReportedUser,
                RequestId = request.RequestId,
                RequestStatus = new ()
                {
                    Status = request.RequestStatus.Status,
                    RequestStatusId = request.RequestStatusId,
                    Requests = null
                },
                RequestType = new()
                {
                    RequestTypeId = request.RequestType.RequestTypeId,
                    Type = request.RequestType.Type,
                },
                UserSender = request.UserSender,
                DeleteUserId = request.DeleteUserId,
                MaxModelHeight = request.MaxModelHeight,
                MaxModelWidth = request.MaxModelWidth,
                MinModelHeight = request.MinModelHeight,
                MinModelWidth = request.MinModelWidth,
                ReportedUserId = request.ReportedUserId,
                RequestStatusId = request.RequestStatusId,
                RequestStatusReason = request.RequestStatusReason,
                RequestTypeId = request.RequestTypeId,
                UserSenderId = request.UserSenderId,
                UserTextData = request.UserTextData,
                RequestStatusReasonId = request.RequestStatusReasonId
            })
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
        var existingEntity = await context.Requests.AsQueryable()
            .Include(existing => existing.RequestStatus)
            .Include(existing => existing.RequestType)
            .FirstAsync(existing => existing.RequestId == request.RequestId);

        existingEntity.RequestId = request.RequestId;
        existingEntity.UserTextData = request.UserTextData;
        existingEntity.UserSenderId = request.UserSenderId;
        existingEntity.RequestTypeId = request.RequestTypeId;
        existingEntity.ReportedUserId = request.ReportedUserId;
        existingEntity.DeleteUserId = request.DeleteUserId;
        existingEntity.ModelId = request.ModelId;
        existingEntity.Description = request.Description;
        existingEntity.LocationX = request.LocationX;
        existingEntity.LocationY = request.LocationY;
        existingEntity.MinModelHeight = request.MinModelHeight;
        existingEntity.MinModelWidth = request.MinModelWidth;
        existingEntity.MaxModelHeight = request.MaxModelHeight;
        existingEntity.MaxModelWidth = request.MaxModelWidth;
        existingEntity.RequestStatusId = request.RequestStatusId;
        existingEntity.RequestStatusReasonId = request.RequestStatusReasonId;
        existingEntity.DeleteUser = request.DeleteUser;
        existingEntity.ReportedUser = request.ReportedUser;
        existingEntity.RequestStatus  = await GetRequestStatusByIdAsync(request.RequestStatus.RequestStatusId);
        existingEntity.RequestStatusReason  = request.RequestStatusReason;
        existingEntity.RequestType = await GetRequestTypeByIdAsync(request.RequestType.RequestTypeId);
        //existingEntity.UserSender = request.UserSender;
        existingEntity.PrintMaterials  = request.PrintMaterials;
        
        context.Requests.Update(existingEntity);
        await context.SaveChangesAsync();
    }

    public async Task AddPrinterAsync(Request request)
    {
        await context.Requests.AddAsync(request);
        await context.SaveChangesAsync();
    }

    public async Task EditPrinterAsync(Request request)
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
    
    public async Task<RequestType> GetRequestTypeByIdAsync(int requestTypeId)
    {
        var requestType = await context
            .RequestTypes
            .AsQueryable()
            .FirstOrDefaultAsync(requestType => requestType.RequestTypeId == requestTypeId);

        return requestType;
    }
    
    public async Task<RequestStatus> GetRequestStatusByIdAsync(int requestStatusId)
    {
        var requestStatus = await context
            .RequestStatuses
            .AsQueryable()
            .FirstOrDefaultAsync(requestType => requestType.RequestStatusId == requestStatusId);

        return requestStatus;
    }
}
