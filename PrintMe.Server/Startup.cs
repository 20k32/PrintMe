using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Logic.Authentication;
using PrintMe.Server.Logic.Services;
using PrintMe.Server.Persistence;
using PrintMe.Server.Persistence.Repository;


namespace PrintMe.Server;

public class Startup
{
    public IConfiguration Configuration { get; }
    public void ConfigureServices(IServiceCollection services, ConfigurationManager manager)
    {
        services.AddRouting(options => options.LowercaseUrls = true);
        
        services.AddDbContext<PrintMeDbContext>(options =>
            options.UseNpgsql("Host=localhost;Port=5432;Database=printme_db;Username=postgres;Password=superuser",
                builder => builder.MigrationsAssembly(Assembly.GetExecutingAssembly()!.FullName)), ServiceLifetime.Singleton);

        services.AddRepositories().AddDatabaseServices();
        
        services.ConfigureAuthentication(manager)
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new()
                    {
                        Title = "PrintMe API",
                        Version = "v1"
                    }
                );

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "PrintMeAPI.xml");
                c.IncludeXmlComments(filePath);
            })
            .AddControllers();
    }

    public void Configure(IApplicationBuilder builder, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            builder.UseSwagger();
            builder.UseSwaggerUI();
        }
        builder.UseAuthentication();
        
        builder.UseRouting();

        builder.UseAuthorization();
        
        builder.UseCors(policy =>
        {
            policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });

        builder.UseHttpsRedirection();
        
        builder.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action}/{id?}");
        });
    }
}