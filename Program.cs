using CommodoreProject_Backend.Controllers;
using CommodoreProject_Backend.Extensions;
using CommodoreProject_Backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

// Extension Methods, städad kod med bättre struktur
builder.Services.AddSwaggerExplorer()
                .InjectDbContext(builder.Configuration)
                .AddIdentityHandlersAndStores()
                .ConfigureIdentityOptions()
                .AddIdentityAuth(builder.Configuration);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var app = builder.Build();

app.ConfigureSwaggerExplorer()
   .ConfigCORS(builder.Configuration)
   .AddIdentityAuthMiddlewares();

app.MapControllers();

app.MapGroup("/api")
   .MapIdentityApi<AppUser>();
app.MapGroup("/api")
   .MapIdentityUserEndpoints()
   .MapAccountEndpoints();

app.UseHttpsRedirection();

app.Run();