using CommodoreProject_Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace CommodoreProject_Backend.Extensions;

public static class EntityFrameworkCoreExtensions
{
    public static IServiceCollection InjectDbContext(this IServiceCollection services, IConfiguration config)
    {
        // Add services to the container.
        services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
        return services;
    }
}