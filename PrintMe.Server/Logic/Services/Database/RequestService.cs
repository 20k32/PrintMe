using AutoMapper;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository;

namespace PrintMe.Server.Logic.Services.Database;

public class RequestService(RequestRepository repository, IMapper mapper)
{
    public async Task<RequestDto> GetRequestByIdAsync(int id)
    {
        var request = await repository.GetRequestByIdAsync(id);

        if (request is null)
        {
            throw new NotFoundRequestInDbException();
        }
        
        return mapper.Map<RequestDto>(request);
    }

    internal async Task<IEnumerable<RequestDto>> GetRequestsByUserIdAsync(int userId)
    {
        var requests = await repository.GetRequestsByUserIdAsync(userId);
        return mapper.Map<IEnumerable<RequestDto>>(requests);
    }
    
    public async Task AddPrinterRequestAsync(AddPrinterRequest addRequest, int id)
    {
        addRequest.UserId = id;
        var request = addRequest.MapAddPrinterRequestToDto().MapDtoToAddRequest();
        await repository.AddPrinterAsync(request);
    }
    
    public async Task EditPrinterRequestAsync(EditPrinterRequest editRequest, int id)
    {
        editRequest.UserId = id;
        var request = editRequest.MapEditPrinterRequestToDto().MapDtoToEditRequest();
        await repository.EditPrinterAsync(request);
    }
}