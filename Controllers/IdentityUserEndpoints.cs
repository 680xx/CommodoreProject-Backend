﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CommodoreProject_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CommodoreProject_Backend.Controllers;

public class UserRegistrationModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
}

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public static class IdentityUserEndpoints
{
    public static IEndpointRouteBuilder MapIdentityUserEndpoints(this IEndpointRouteBuilder app)
    {
        // Endpoints (Minimal API)
        app.MapPost("/signUp", CreateUser);
        app.MapPost("/signIn", SignIn);
        return app;
    }

    [AllowAnonymous]
    private static async Task<IResult> CreateUser(UserManager<AppUser> userManager,
        [FromBody] UserRegistrationModel userRegistrationModel)
    {
        AppUser user = new AppUser()
        {
            UserName = userRegistrationModel.Email,
            Email = userRegistrationModel.Email,
            FullName = userRegistrationModel.FullName,
        };
        var result = await userManager.CreateAsync(user, userRegistrationModel.Password);
        await userManager.AddToRoleAsync(user, userRegistrationModel.Role);
        if (result.Succeeded)
            return Results.Ok(result);
        else
            return Results.BadRequest(result);
    }

    [AllowAnonymous]
    private static async Task<IResult> SignIn(UserManager<AppUser> userManager,
        [FromBody] LoginModel loginModel,
        IOptions<AppSettings> appSettings)
    {
        var user = await userManager.FindByEmailAsync(loginModel.Email);
        if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
        {
            var roles = await userManager.GetRolesAsync(user);
            var signInKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(appSettings.Value.JWTSecret)
            );
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
            {
                new Claim("userID", user.Id.ToString()),
                new Claim(ClaimTypes.Role,roles.First())
            });
            // Om man vill ha ett krav som inte är obligatoriskt att användaren har.
            // if (user.libraryID != null)
            //     claims.AddClaim(new Claim("libraryID", user.libraryID.ToString()!));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(
                    signInKey,
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return Results.Ok(new { token });
        }
        else
            return Results.BadRequest(new { message = "Username or password is incorrect." });
    }
}
