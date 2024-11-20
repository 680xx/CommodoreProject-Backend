using CommodoreProject_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace CommodoreProject_Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } // Example DbSet
}