using PrintMe.Server.Models.Exceptions;

namespace PrintMe.Server.Logic.Strategies;

public static class RequestApprovalStrategyFactory
{
    private static readonly Dictionary<string, IRequestApprovalStrategy> Strategies = new()
    {
        { "PRINTER_APPLICATION", new PrinterApplicationStrategy() },
        { "PRINTER_DESCRIPTION_CHANGE", new PrinterDescriptionChangeStrategy() },
        { "USER_REPORT", new UserReportStrategy() },
        { "ACCOUNT_DELETION", new AccountDeletionStrategy() }
    };

    public static IRequestApprovalStrategy GetStrategy(string requestType)
    {
        if (Strategies.TryGetValue(requestType, out var strategy))
        {
            return strategy;
        }
        throw new NotFoundStrategyException();
    }
}
