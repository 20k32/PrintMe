using PrintMe.Server.Persistence.Repository.Interfaces;

namespace PrintMe.Server.Persistence.Repository
{
    public static class DiExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection collection) =>
            collection
                .AddScoped<IRolesRepository, RolesRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IRequestRepository, RequestRepository>()
                .AddScoped<IPrinterRepository, PrinterRepository>()
                .AddScoped<IOrderRepository, OrderRepository>();
    }
}