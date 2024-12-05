using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Models.DTOs.RequestDto;

namespace PrintMe.Server.Logic.Strategies;

public class PrinterApplicationStrategy : RequestApprovalStrategyBase
{
    public override async Task ApproveRequestAsync(RequestDto request, IServiceProvider provider)
    {
        await base.ApproveRequestAsync(request, provider);
        var requestService = provider.GetRequiredService<RequestService>();
        var printerService = provider.GetRequiredService<PrinterService>();
        await printerService.AddPrinterAsync(await requestService.ToPrinterDtoAsync(request.RequestId));
        await requestService.UpdateRequestAsync(request);
    }
}

public class PrinterDescriptionChangeStrategy : RequestApprovalStrategyBase
{
    public override Task ApproveRequestAsync(RequestDto request, IServiceProvider provider)
    {
        throw new NotImplementedException();
    }
}

public class UserReportStrategy : RequestApprovalStrategyBase
{
    public override Task ApproveRequestAsync(RequestDto request, IServiceProvider provider)
    {
        throw new NotImplementedException();
    }
}

public class AccountDeletionStrategy : RequestApprovalStrategyBase
{
    public override Task ApproveRequestAsync(RequestDto request, IServiceProvider provider)
    {
        throw new NotImplementedException();
    }
}
