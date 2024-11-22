using CommodoreProject_Backend.Data;
using CommodoreProject_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommodoreProject_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }
    
    // Get all users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }
    
    // Get a user by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound($"User with ID {id} not found");
        }

        return user;
    }


    // Create a new user
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        // Ensure the Id is 0 and therefore is auto increment
        user.Id = 0;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
    }

    // Update an existing user
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User updatedUser)
    {
        if (id != updatedUser.Id)
        {
            return BadRequest("User ID mismatch");
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound($"User with ID {id} not found");
        }

        // Update the user properties
        user.Name = updatedUser.Name;
        user.Age = updatedUser.Age;
        user.Email = updatedUser.Email;
        user.Password = updatedUser.Password;
        user.Gender = updatedUser.Gender;
        user.Phone = updatedUser.Phone;
        user.Role = updatedUser.Role;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Users.Any(u => u.Id == id))
            {
                return NotFound($"User with ID {id} no longer exists");
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // Delete a user
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound($"User with ID {id} not found");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
