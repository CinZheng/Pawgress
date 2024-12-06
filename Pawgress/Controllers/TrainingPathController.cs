using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Data;

[ApiController]
[Route("api/[controller]")]
public class TrainingPathController : Controller
{
    private readonly ApplicationDbContext _context;

    public TrainingPathController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAllTrainingPaths()
    {
        var trainingPaths = _context.TrainingPaths.ToList();
        return Ok(trainingPaths);
    }
}