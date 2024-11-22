namespace PrintMe.Server.Logic.Registration;

public static class DIExtentions
{
    public static IServiceCollection ConfigureRegistration(this IServiceCollection collection) =>
        collection.AddSingleton<UserRegistrationLogic>();
}