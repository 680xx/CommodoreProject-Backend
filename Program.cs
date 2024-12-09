using CommodoreProject_Backend.Controllers;
using CommodoreProject_Backend.Extensions;
using CommodoreProject_Backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Extension Methods, städad kod med bättre struktur
builder.Services.AddControllers();

builder.Services.AddSwaggerExplorer()
                .InjectDbContext(builder.Configuration)
                .AddAppConfig(builder.Configuration)
                .AddIdentityHandlersAndStores()
                .ConfigureIdentityOptions()
                .AddIdentityAuth(builder.Configuration);

var app = builder.Build();

app.ConfigureSwaggerExplorer()
   .ConfigCORS(builder.Configuration)
   .AddIdentityAuthMiddlewares();

app.MapControllers();
app.MapGroup("/api")
   .MapIdentityApi<AppUser>();
app.MapGroup("/api")
   .MapIdentityUserEndpoints()
   .MapAccountEndpoints()
   .MapAuthorizationDemoEndpoints();

app.UseHttpsRedirection();

app.Run();