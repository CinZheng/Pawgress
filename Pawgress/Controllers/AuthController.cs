using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pawgress.Data;
using Pawgress.Models;
using Microsoft.Extensions.Configuration;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration; // lezen van Issuer, Audience en Key uit appsettings

    public AuthController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        // check if user exists
        if (_context.Users.Any(u => u.Email == registerDto.Email))
        {
            return BadRequest("Email is al in gebruik.");
        }

        // add new user
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password), // hash password
            Role = "User" // standard role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Account succesvol aangemaakt! U kunt nu inloggen.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        // search user by email
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

        // generate JWT token
        var token = GenerateJwtToken(user);

        // login success
        return Ok(new
        {
            Message = "Succesvol ingelogd!",
            Token = token,
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        });
    }

    [Authorize]
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // aangepaste claimtype voor unieke ID
        return Ok($"Je bent ingelogd met ID: {userId}");
    }

    private string GenerateJwtToken(User user)
    {
        // claims
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        // key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])); // lees uit appsettings
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // token
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"], // lees uit appsettings
            audience: _configuration["Jwt:Audience"], // lees uit appsettings
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
