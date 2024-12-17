using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Services;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : Controller
    {
        private readonly NoteService _service;

        public NoteController(NoteService service)
        {
            _service = service;
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetNotesByUser(Guid userId)
        {
            return Ok(_service.GetNotesByUser(userId));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var note = _service.GetById(id);
            return note == null ? NotFound("Niet gevonden.") : Ok(note);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Note note)
        {
            return Ok(_service.Create(note));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return _service.Delete(id) ? Ok("Succesvol verwijderd.") : NotFound("Niet gevonden.");
        }
    }
}
