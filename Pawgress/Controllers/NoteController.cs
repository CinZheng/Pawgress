using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Data;

[ApiController]
[Route("api/[controller]")]
public class NoteController : Controller
{
    private readonly ApplicationDbContext _context;

    public NoteController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAllNotes()
    {
        var notes = _context.Notes.ToList();
        return Ok(notes);
    }
}