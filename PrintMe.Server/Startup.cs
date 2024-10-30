namespace PrintMe.Server;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer()
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

        builder.UseRouting();

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