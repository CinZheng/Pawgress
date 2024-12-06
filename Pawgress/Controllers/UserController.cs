using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Data;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _context.Users.ToList();
        return Ok(users);
    }
}