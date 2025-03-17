using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs.PrinterDto;
using PrintMe.Server.Models.DTOs.RequestDto;
using PrintMe.Server.Models.Filters;

namespace PrintMe.Server.Logic.Services.Database.Interfaces;

public interface IRequestService
{
    Task<IEnumerable<RequestDto>> GetAllRequestsAsync(RequestFilter filter = null);
    Task<RequestDto> GetRequestByIdAsync(int id);
    Task<IEnumerable<RequestDto>> GetRequestsByUserIdAsync(int userId, RequestFilter filter = null);
    Task UpdateRequestAsync(RequestDto request);
    Task AddPrinterRequestAsync(AddPrinterRequest addRequest);
    Task EditPrinterRequestAsync(EditPrinterRequest editRequest, int id);
    Task<PrinterDto> ToPrinterDtoAsync(int id);
    Task ApproveRequestAsync(RequestDto request, IServiceProvider provider);
    Task DeclineRequestAsync(RequestDto request, string reason);
    // Task<string> GetRequestTypeNameByIdAsync(int requestTypeId); //private
}