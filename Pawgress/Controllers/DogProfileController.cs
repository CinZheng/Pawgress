using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Data;

[ApiController]
[Route("api/[controller]")]
public class DogProfileController : Controller
{
    private readonly ApplicationDbContext _context;

    public DogProfileController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAllDogProfiles()
    {
        var dogProfiles = _context.DogProfiles.ToList();
        return Ok(dogProfiles);
    }
}