using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Data;

[ApiController]
[Route("api/[controller]")]
public class LibraryController : Controller
{
    private readonly ApplicationDbContext _context;

    public LibraryController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAllLibraries()
    {
        var libraries = _context.Libraries.ToList();
        return Ok(libraries);
    }
}