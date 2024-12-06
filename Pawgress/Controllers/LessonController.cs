using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Data;

[ApiController]
[Route("api/[controller]")]
public class LessonController : Controller
{
    private readonly ApplicationDbContext _context;

    public LessonController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAllLessons()
    {
        var lessons = _context.Lessons.ToList();
        return Ok(lessons);
    }
}