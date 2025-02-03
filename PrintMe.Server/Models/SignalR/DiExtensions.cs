using Microsoft.AspNetCore.SignalR;

namespace PrintMe.Server.Models.SignalR;

public static class DiExtensions
{
    public static IServiceCollection AddUserIdProvider(this IServiceCollection collection)
        => collection.AddSingleton<IUserIdProvider, UserIdProvider>();
}