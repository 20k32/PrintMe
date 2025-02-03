using Microsoft.AspNetCore.SignalR;
using PrintMe.Server.Logic.Authentication;

namespace PrintMe.Server.Models.SignalR;

public class UserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
        => connection?.User.FindFirst((CustomClaimTypes.USER_ID))?.Value ?? string.Empty;
}