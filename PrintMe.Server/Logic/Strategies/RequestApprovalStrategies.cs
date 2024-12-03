using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Models.DTOs.RequestDto;

namespace PrintMe.Server.Logic.Strategies;

public class PrinterApplicationStrategy : IRequestApprovalStrategy
{
    public async Task ApproveRequestAsync(RequestDto request, IServiceProvider provider)
    {
        var requestService = provider.GetRequiredService<RequestService>();
        var printerService = provider.GetRequiredService<PrinterService>();
        await printerService.AddPrinterAsync(await requestService.ToPrinterDtoAsync(request.RequestId));
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
