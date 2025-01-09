using PrintMe.Server.Models.Exceptions;

namespace PrintMe.Server.Logic.Strategies;

public static class RequestApprovalStrategyFactory
{
    private static readonly Dictionary<string, IRequestApprovalStrategy> Strategies = new()
    {
        { "PrinterApplication", new PrinterApplicationStrategy() },
        { "PrinterDescriptionChanging", new PrinterDescriptionChangeStrategy() },
        { "UserReport", new UserReportStrategy() },
        { "AccountDeletion", new AccountDeletionStrategy() }
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
