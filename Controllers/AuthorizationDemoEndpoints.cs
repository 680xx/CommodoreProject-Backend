using Microsoft.AspNetCore.Authorization;

namespace CommodoreProject_Backend.Controllers;

public static class AuthorizationDemoEndpoints
{
    public static IEndpointRouteBuilder MapAuthorizationDemoEndpoints(this IEndpointRouteBuilder app)
    {
        
        app.MapGet("/AdminOnly", AdminOnly);
        app.MapGet("/AdminOrOwner", AdminOrOwner);
        return app;
    }

    [Authorize(Roles ="Admin")]
    private static string AdminOnly()
    {
        return "Admin Only";
    }

    [Authorize(Roles ="Admin, Owner")]
    private static string AdminOrOwner()
    {
        return "Admin Or Owner";
    }
}