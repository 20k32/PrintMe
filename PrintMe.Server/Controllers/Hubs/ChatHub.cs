using Microsoft.AspNetCore.SignalR;
using PrintMe.Server.Models.Api.ApiRequest;

namespace PrintMe.Server.Controllers.Hubs;


public sealed class ChatHub : Hub
{
    public async Task MessageReceived(SendMessageRequest parameter)
    {
        if (Context.UserIdentifier is { } userId)
        {
            if (string.IsNullOrWhiteSpace(parameter.SenderId))
            {
                parameter.SenderId = Context.UserIdentifier;
            }
            
            await Clients.Users(parameter.ReceiverId).SendAsync(nameof(MessageReceived), parameter);
        }
    }
}