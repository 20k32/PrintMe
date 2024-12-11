using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PrintMe.Server.Logic.Authentication;
using PrintMe.Server.Logic.Services;
using PrintMe.Server.Persistence;
using PrintMe.Server.Persistence.Repository;


namespace PrintMe.Server;

public class Startup
{
    public void ConfigureServices(IServiceCollection services, ConfigurationManager manager)
    {
        services.AddRouting(options => options.LowercaseUrls = true);
        
        services.AddAutoMapper(typeof(Startup).Assembly);
        
        services.AddDbContext<PrintMeDbContext>(options =>
            options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING_PRINTME_DB"),
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
            });
            
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders("Access-Control-Allow-Origin");
            });
        });
    }

    public void Configure(IApplicationBuilder builder, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            builder.UseSwagger();
            builder.UseSwaggerUI();
        }

        builder.UseHttpsRedirection();

        builder.UseCors();

        builder.UseAuthentication();
        builder.UseRouting();
        builder.UseAuthorization();
        
        builder.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action}/{id?}");
        });
    }
}