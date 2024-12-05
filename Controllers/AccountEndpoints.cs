using System.Security.Claims;
using CommodoreProject_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CommodoreProject_Backend.Controllers;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        
        app.MapGet("/UserProfile", GetUserProfile);
        return app;
    }

    [Authorize]
    private static async Task<IResult> GetUserProfile(ClaimsPrincipal user, UserManager<AppUser> userManager)
    {
        string userID = user.Claims.First(x => x.Type == "UserID").Value;
        var userDetails = await userManager.FindByIdAsync(userID);
        return Results.Ok(
            new
            {
                Email = userDetails?.Email,
                FullName = userDetails?.FullName,
            });
    }
};