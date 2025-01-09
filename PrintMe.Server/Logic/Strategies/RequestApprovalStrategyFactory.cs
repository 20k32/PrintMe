using PrintMe.Server.Constants;
using PrintMe.Server.Models.Exceptions;

namespace PrintMe.Server.Logic.Strategies;

public static class RequestApprovalStrategyFactory
{
    private static readonly Dictionary<string, IRequestApprovalStrategy> Strategies = new()
    {
        { DbConstants.RequestType.PrinterApplication, new PrinterApplicationStrategy() },
        { DbConstants.RequestType.PrinterDescriptionChanging, new PrinterDescriptionChangeStrategy() },
        { DbConstants.RequestType.UserReport, new UserReportStrategy() },
        { DbConstants.RequestType.AccountDeletion, new AccountDeletionStrategy() }
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
