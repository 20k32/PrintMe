using PrintMe.Server.Logic.Authentication;

namespace PrintMe.Server;

public class Startup
{
    public void ConfigureServices(IServiceCollection services, ConfigurationManager manager)
    {
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
                .AllowCredentials()
                .WithOrigins("http://localhost:5173")
                .SetIsOriginAllowed(origin => true)
                .WithExposedHeaders("Access-Control-Allow-Origin");
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