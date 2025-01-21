using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Models.Filters;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository;

public class RequestRepository(PrintMeDbContext context)
{
	public async Task<IEnumerable<Request>> GetAllRequestsAsync(RequestFilter filter = null)
	{
		if (filter is null || filter.StatusId == 0 && filter.TypeId == 0)
		{
			return await context
				.Requests
				.AsQueryable()
				.ToListAsync();
		}

		var statusId = filter.StatusId ?? 0;
		var typeId = filter.TypeId ?? 0;
		if (statusId != 0 && typeId != 0)
		{
			return await context
				.Requests
				.AsQueryable()
				.Where(request => request.RequestStatusId == statusId && request.RequestTypeId == typeId)
				.ToListAsync();
		}

		if (statusId == 0 && typeId != 0)
		{
			return await context
				.Requests
				.AsQueryable()
				.Where(request => request.RequestTypeId == typeId)
				.ToListAsync();
		}

		if (typeId == 0 && statusId != 0)
		{
			return await context
				.Requests
				.AsQueryable()
				.Where(request => request.RequestStatusId == statusId)
				.ToListAsync();
		}

		return await context
			.Requests
			.AsQueryable()
			.ToListAsync();
	}

	public async Task<Request> GetRequestByIdAsync(int requestId)
	{
		return await context
			.Requests
			.AsQueryable()
			.Include(request => request.PrintMaterials)
			.FirstOrDefaultAsync(request => request.RequestId == requestId);
	}

	public async Task<IEnumerable<Request>> GetRequestsByUserIdAsync(int userId, RequestFilter filter = null)
	{
		if (filter is null || filter.StatusId == 0 && filter.TypeId == 0)
		{
			return await context
				.Requests
				.AsQueryable()
				.Where(request => request.UserSenderId == userId)
				.ToListAsync();
		}

		var statusId = filter.StatusId ?? 0;
		var typeId = filter.TypeId ?? 0;
		if (statusId != 0 && typeId != 0)
		{
			return await context
				.Requests
				.AsQueryable()
				.Where(request => request.UserSenderId == userId && request.RequestStatusId == statusId &&
				                  request.RequestTypeId == typeId)
				.ToListAsync();
		}

		if (statusId == 0 && typeId != 0)
		{
			return await context
				.Requests
				.AsQueryable()
				.Where(request => request.UserSenderId == userId && request.RequestTypeId == typeId)
				.ToListAsync();
		}

		if (typeId == 0 && statusId != 0)
		{
			return await context
				.Requests
				.AsQueryable()
				.Where(request => request.UserSenderId == userId && request.RequestStatusId == statusId)
				.ToListAsync();
		}

		throw new Exception("Invalid filter");
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