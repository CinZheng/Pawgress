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

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        // searchuser by email
        var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Email);
        if (user == null)
        {
            return Unauthorized("Ongeldige inloggegevens.");
        }

        // check password
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return Unauthorized("Ongeldige inloggegevens.");
        }

        // login success
        return Ok(new
        {
            Message = "Succesvol ingelogd!",
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        });
    }

}
