using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Logic.Services.Database.Interfaces;
using PrintMe.Server.Models.DTOs.PrinterDto;

namespace PrintMe.Server.Logic.Services
{
    public static class DiExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection collection) =>
            collection
                .AddScoped<IUserService, UserService>()
                .AddScoped<IRequestService, RequestService>()
                .AddScoped<IPrinterService, PrinterService>()
                .AddScoped<IOrderService,OrderService>()
                .AddScoped<IVerificationService, VerificationService>();
    }
}