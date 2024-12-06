using Microsoft.AspNetCore.Mvc;
using Pawgress.Data;
using Pawgress.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        // check if user exists
        if (_context.Users.Any(u => u.Email == registerDto.Email))
        {
            return BadRequest("Email is al in gebruik ):");
        }

        // add new user
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password), // hash passworrd
            Role = "User" // standard role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Account succesvol aangemaakt!");
    }
}
