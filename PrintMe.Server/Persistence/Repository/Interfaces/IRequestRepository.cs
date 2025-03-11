using PrintMe.Server.Models.Filters;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository.Interfaces;

public interface IRequestRepository
{
    Task<IEnumerable<Request>> GetAllRequestsAsync(RequestFilter filter = null);
    Task<Request> GetRequestByIdAsync(int requestId);
    Task<IEnumerable<Request>> GetRequestsByUserIdAsync(int userId, RequestFilter filter = null);
    Task<int> GetRequestStatusReasonIdByNameAsync(string reason);
    Task UpdateRequestAsync(Request request);
    Task AddPrinterRequestAsync(Request request);
    Task EditPrinterRequestAsync(Request request);
    Task<string> GetRequestTypeNameByIdAsync(int requestTypeId);
    Task AddPrinterRequestMaterialsAsync(int requestId, IEnumerable<int> materialIds);
}