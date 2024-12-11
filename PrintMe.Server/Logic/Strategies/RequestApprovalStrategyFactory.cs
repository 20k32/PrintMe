using PrintMe.Server.Models.Exceptions;

namespace PrintMe.Server.Logic.Strategies;

// todo: inject to di as singleton
public class RequestApprovalStrategyFactory
{
    private readonly Dictionary<string, IRequestApprovalStrategy> _strategies = new()
    {
        { "PRINTER_APPLICATION", new PrinterApplicationStrategy() },
        { "PRINTER_DESCRIPTION_CHANGE", new PrinterDescriptionChangeStrategy() },
        { "USER_REPORT", new UserReportStrategy() },
        { "ACCOUNT_DELETION", new AccountDeletionStrategy() }
    };

    public IRequestApprovalStrategy GetStrategy(string requestType)
    {
        if (_strategies.TryGetValue(requestType, out var strategy))
        {
            return strategy;
        }
        throw new NotFoundStrategyException();
    }
}
