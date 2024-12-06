using PrintMe.Server.Models.DTOs.RequestDto;

namespace PrintMe.Server.Logic.Strategies;

public interface IRequestApprovalStrategy
{
    Task ApproveRequestAsync(RequestDto request, IServiceProvider provider);
}
