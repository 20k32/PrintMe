using DotNetEnv;
using PrintMe.Server.Controllers.Hubs;
using PrintMe.Server.Logic.Authentication;
using PrintMe.Server.Models.Authentication;
using PrintMe.Server.Persistence;

namespace PrintMe.Server;

public static class Program
{
    private static void Main(string[] args)
    {
        Env.Load();
        
        var builder = WebApplication.CreateBuilder(args);
        
        var startup = new Startup();

        startup.ConfigureServices(builder.Services, builder.Configuration);
        
        var app = builder.Build();
        startup.Configure(app, app.Environment);
        app.MapControllers();
        
        app.MapHub<ChatHub>("/message");
        
        app.Run();
    }
}