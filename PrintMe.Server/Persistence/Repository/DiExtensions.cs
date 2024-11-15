namespace PrintMe.Server.Persistence.Repository
{
    public static class DiExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection collection) =>
            collection.AddSingleton<UserRepository>();
    }
}