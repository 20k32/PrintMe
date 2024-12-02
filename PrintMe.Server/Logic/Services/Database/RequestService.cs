using AutoMapper;
using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository;

namespace PrintMe.Server.Logic.Services.Database;

public class RequestService(RequestRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<RequestDto>> GetAllRequestsAsync()
    {
        var requests = await repository.GetAllRequestsAsync();

        if (!requests.Any())
        {
            throw new NotFoundRequestInDbException();
        }

        return mapper.Map<IEnumerable<RequestDto>>(requests);
    }

    public async Task<RequestDto> GetRequestByIdAsync(int id)
    {
        var request = await repository.GetRequestByIdAsync(id);

        if (request is null)
        {
            throw new NotFoundRequestInDbException();
        }
        
        return mapper.Map<RequestDto>(request);
    }

    public async Task<IEnumerable<RequestDto>> GetRequestsByStatusIdAsync(int status)
    {
        var requests = await repository.GetRequestsByStatusIdAsync(status);

        if (!requests.Any())
        {
            throw new NotFoundRequestInDbException();
        }

        return mapper.Map<IEnumerable<RequestDto>>(requests);
    }

    internal async Task<IEnumerable<RequestDto>> GetRequestsByUserIdAsync(int userId)
    {
        var requests = await repository.GetRequestsByUserIdAsync(userId);
        return mapper.Map<IEnumerable<RequestDto>>(requests);
    }

    public async Task<int> GetRequestStatusIdByNameAsync(string status)
    {
        try
        {
            return await repository.GetRequestStatusIdByNameAsync(status);
        }
        catch (Exception)
        {
            throw new NotFoundStatusInDb();
        }
    }

    public async Task<int> GetRequestStatusReasonIdByNameAsync(string reason)
    {
        try
        {
            return await repository.GetRequestStatusReasonIdByNameAsync(reason);
        }
        catch (Exception)
        {
            throw new NotFoundStatusReasonInDb();
        }
    }

    public async Task UpdateRequestAsync(RequestDto request)
    {
        await repository.UpdateRequestAsync(mapper.Map<Request>(request));
    }
}
