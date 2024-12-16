using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Models.DTOs.PrinterDto;

namespace PrintMe.Server.Logic.Services
{
    public static class DiExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection collection) =>
            collection
                .AddScoped<UserService>()
                .AddScoped<RequestService>()
                .AddScoped<PrinterService>()
                .AddScoped<OrderService>();
    }
}