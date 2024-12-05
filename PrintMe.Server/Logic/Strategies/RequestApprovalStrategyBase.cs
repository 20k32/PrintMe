using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Models.DTOs.RequestDto;

namespace PrintMe.Server.Logic.Strategies;

public abstract class RequestApprovalStrategyBase : IRequestApprovalStrategy
{
    public virtual async Task ApproveRequestAsync(RequestDto request, IServiceProvider provider)
    {
        var requestService = provider.GetRequiredService<RequestService>();

        var approvedStatusId = await requestService.GetRequestStatusIdByNameAsync("APPROVED");
        if (request.RequestStatusId == approvedStatusId)
        {
            throw new ArgumentException("Request is already approved");
        }

        request.RequestStatusId = approvedStatusId;
        request.RequestStatusReasonId = null;
    }
}