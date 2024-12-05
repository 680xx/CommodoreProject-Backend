using Microsoft.AspNetCore.Authorization;

namespace CommodoreProject_Backend.Controllers;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        
        app.MapGet("/UserProfile", GetUserProfile);
        return app;
    }

    [Authorize]
    private static string GetUserProfile()
    {
        return "User profile";
    }
    
};