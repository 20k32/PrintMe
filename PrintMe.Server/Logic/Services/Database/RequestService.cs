using AutoMapper;
using PrintMe.Server.Logic.Strategies;
using PrintMe.Server.Models.Api;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs.PrinterDto;
using PrintMe.Server.Models.DTOs.RequestDto;
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
            throw new NotFoundRequestStatusInDb();
        }
    }

    private async Task<int> GetRequestStatusReasonIdByNameAsync(string reason)
    {
        try
        {
            return await repository.GetRequestStatusReasonIdByNameAsync(reason.ToUpper());
        }
        catch (Exception)
        {
            throw new NotFoundRequestStatusReasonInDbException();
        }
    }

    public async Task UpdateRequestAsync(RequestDto request)
    {
        await repository.UpdateRequestAsync(mapper.Map<Request>(request));
    }

    public async Task AddPrinterRequestAsync(AddPrinterRequest addRequest, int id)
    {
        addRequest.UserId = id;
        var requestDto = mapper.Map<RequestDto>(addRequest);
        var request = mapper.Map<Request>(requestDto);
        await repository.AddPrinterAsync(request);
    }

    public async Task EditPrinterRequestAsync(EditPrinterRequest editRequest, int id)
    {
        editRequest.UserId = id;
        var requestDto = mapper.Map<RequestDto>(editRequest);
        var request = mapper.Map<Request>(requestDto);
        await repository.EditPrinterAsync(request);
    }

    private async Task<string> GetRequestTypeNameByIdAsync(int requestTypeId)
    {
        try
        {
            return await repository.GetRequestTypeNameByIdAsync(requestTypeId);
        }
        catch (Exception)
        {
            throw new NotFoundRequestTypeInDb();
        }
    }

    public async Task<PrinterDto> ToPrinterDtoAsync(int id)
    {
        var request = await repository.GetRequestByIdAsync(id);
        return mapper.Map<PrinterDto>(request);
    }

    public async Task ApproveRequestAsync(RequestDto request, IServiceProvider provider)
    {
        var requestService = provider.GetRequiredService<RequestService>();

        var requestType = await requestService.GetRequestTypeNameByIdAsync(request.RequestTypeId);
        var strategyFactory = new RequestApprovalStrategyFactory();

        var strategy = strategyFactory.GetStrategy(requestType);
        await strategy.ApproveRequestAsync(request, provider);
    }

    public async Task DeclineRequestAsync(RequestDto request, string reason)
    {
        var declinedStatusId = await GetRequestStatusIdByNameAsync("DECLINED");
        var reasonId = await GetRequestStatusReasonIdByNameAsync(reason);

        if (request.RequestStatusId == declinedStatusId)
        {
            throw new AlreadyApprovedRequestException();
        }

        request.RequestStatusId = declinedStatusId;
        request.RequestStatusReasonId = reasonId;

        await UpdateRequestAsync(request);
    }
}
