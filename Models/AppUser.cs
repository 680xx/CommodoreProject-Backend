using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CommodoreProject_Backend.Models;

public class AppUser:IdentityUser
{
    [PersonalData]
    [Column(TypeName ="varchar(150)")]
    public string? FullName { get; set; }
}