namespace PrintMe.Server.Persistence.Repository
{
    public static class DiExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection collection) =>
            collection
                .AddScoped<RolesRepository>()
                .AddScoped<UserRepository>()
                .AddScoped<RequestRepository>()
                .AddScoped<PrinterRepository>()
                .AddScoped<OrderRepository>();
    }
}