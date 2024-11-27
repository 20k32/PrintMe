using PrintMe.Server.Logic.Services.Database;

namespace PrintMe.Server.Logic.Services
{
    public static class DiExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection collection) =>
            collection
                .AddSingleton<UserService>()
                .AddSingleton<RequestService>();
    }
}