using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Data;

[ApiController]
[Route("api/[controller]")]
public class FolderController : ControllerBase 
{
    private readonly ApplicationDbContext _context;

    public FolderController(ApplicationDbContext context) {
        _context = context; // dependency injected
    }

    // GET: api/Folder/test
    [HttpGet("test")]
    public IActionResult TestDatabase() 
    {
        var folders = _context.Folders.ToList();
        return Ok(
            new { Message = $"aantal folder in db: {folders.Count}"});
    }
}