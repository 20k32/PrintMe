using AutoMapper;
using PrintMe.Server.Logic.Strategies;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs.PrinterDto;
using PrintMe.Server.Models.DTOs.RequestDto;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository;

namespace PrintMe.Server.Logic.Services.Database;

internal class RequestService(RequestRepository repository, IMapper mapper, PrinterRepository printerRepository)
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

    public async Task AddPrinterRequestAsync(AddPrinterRequest addRequest)
    {
        var requestDto = mapper.Map<RequestDto>(addRequest);
        var request = mapper.Map<Request>(requestDto);

        await repository.AddPrinterRequestAsync(request);
        
        await repository.AddPrinterRequestMaterialsAsync(request.RequestId, addRequest.Materials.Select(m => m.PrintMaterialId));
    }

    public async Task EditPrinterRequestAsync(EditPrinterRequest editRequest, int id)
    {
        editRequest.UserId = id;
        var requestDto = mapper.Map<RequestDto>(editRequest);
        var request = mapper.Map<Request>(requestDto);
        await repository.EditPrinterRequestAsync(request);
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
        
        var result = mapper.Map<PrinterDto>(request);

        var modelName = string.Empty;
        
        if (request.ModelId is not null && request.ModelId > 0)
        {
            modelName = (await printerRepository.GetModelByIdAsync(request.ModelId.Value))?.Name 
                        ?? string.Empty;
        }
        
        result.ModelName = modelName;

        var materials = new List<PrintMaterialDto>(request.PrintMaterials.Count);

        foreach (var materialRaw in request.PrintMaterials)
        {
            var materialDto = mapper.Map<PrintMaterialDto>(materialRaw);
            materials.Add(materialDto);
        }
        
        result.Materials = materials;

        result.UserId = request.UserSenderId;

        return result;
    }

    public async Task ApproveRequestAsync(RequestDto request, IServiceProvider provider)
    {

        var approvedStatusId = await GetRequestStatusIdByNameAsync("Approved");
        if (request.RequestStatusId == approvedStatusId)
        {
            throw new AlreadyApprovedRequestException();
        }

        var requestType = await GetRequestTypeNameByIdAsync(request.RequestTypeId);

        var strategy = RequestApprovalStrategyFactory.GetStrategy(requestType);
        await strategy.ApproveRequestAsync(request, provider);

        request.RequestStatusId = approvedStatusId;
        request.RequestStatusReasonId = null;

        await UpdateRequestAsync(request);
    }

    public async Task DeclineRequestAsync(RequestDto request, string reason)
    {
        var declinedStatusId = await GetRequestStatusIdByNameAsync("Declined");
        var reasonId = await GetRequestStatusReasonIdByNameAsync(reason);

        if (request.RequestStatusId == declinedStatusId)
        {
            throw new AlreadyDeclinedRequestException();
        }

        request.RequestStatusId = declinedStatusId;
        request.RequestStatusReasonId = reasonId;

        await UpdateRequestAsync(request);
    }
}
