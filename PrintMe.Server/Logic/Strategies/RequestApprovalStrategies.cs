using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Logic.Services.Database.Interfaces;
using PrintMe.Server.Models.DTOs.RequestDto;

namespace PrintMe.Server.Logic.Strategies;

public class PrinterApplicationStrategy : IRequestApprovalStrategy
{
    public async Task ApproveRequestAsync(RequestDto request, IServiceProvider provider)
    {
        var requestService = provider.GetRequiredService<IRequestService>();
        var printerService = provider.GetRequiredService<IPrinterService>();

        var printerDto = await requestService.ToPrinterDtoAsync(request.RequestId);
        await printerService.AddPrinterAsync(printerDto);
        await requestService.UpdateRequestAsync(request);
    }
}

public class PrinterDescriptionChangeStrategy : IRequestApprovalStrategy
{
    public Task ApproveRequestAsync(RequestDto request, IServiceProvider provider)
    {
        throw new NotImplementedException();
    }
}

public class UserReportStrategy : IRequestApprovalStrategy
{
    public Task ApproveRequestAsync(RequestDto request, IServiceProvider provider)
    {
        throw new NotImplementedException();
    }
}

public class AccountDeletionStrategy : IRequestApprovalStrategy
{
    public Task ApproveRequestAsync(RequestDto request, IServiceProvider provider)
    {
        throw new NotImplementedException();
    }
}
