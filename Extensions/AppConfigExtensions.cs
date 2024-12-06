using CommodoreProject_Backend.Models;

namespace CommodoreProject_Backend.Extensions;

public static class AppConfigExtensions
{
    public static WebApplication ConfigCORS(this WebApplication app, IConfiguration config)
    {
        app.UseCors(options =>
            options.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader());
        return app;
    }
    
    public static IServiceCollection AddAppConfig(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<AppSettings>(config.GetSection("AppSettings"));
        return services;
    }
}