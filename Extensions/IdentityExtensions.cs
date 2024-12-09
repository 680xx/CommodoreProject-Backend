using System.Text;
using CommodoreProject_Backend.Data;
using CommodoreProject_Backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CommodoreProject_Backend.Extensions;

public static class IdentityExtensions
{
    public static IServiceCollection AddIdentityHandlersAndStores(this IServiceCollection services)
    {
        services.AddIdentityApiEndpoints<AppUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();
        return services;
    }

    public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
    {
        // Configure Password Requirements
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredUniqueChars = 1;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
        });
        return services;
    }
    
    public static IServiceCollection AddIdentityAuth(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(y =>
        {
            y.SaveToken = false;
            y.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        config["AppSettings:JWTSecret"]!)),
                ValidateIssuer = false,     // Kollar vilken entitet som utfärdat token.
                ValidateAudience = false,   // Kollar vem som token är avsedd för.
            };
        });
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            
            // Policy/criteria för att kräva att man har "libraryID" och "Ålder under 10".
            // options.AddPolicy("HasLibraryID", policy => policy.RequireClaim("LibraryID"));
            // options.AddPolicy("Under10", policy => policy.RequireAssertion(context =>
            // Int32.Parse(ContextCallback.User.Claims.First(x => x.type=="Age").value)<10));
        });
        return services;
    }
    
    public static WebApplication AddIdentityAuthMiddlewares(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}