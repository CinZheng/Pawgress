using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Services;
using System;
using System.Linq;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : BaseController<Note>
    {
        private readonly NoteService _service;

        public NoteController(NoteService service) : base(service)
        {
            _service = service;
        }

        [HttpGet("dogprofile/{dogProfileId}")]
        public IActionResult GetNotesByDogProfile(Guid dogProfileId)
        {
            var notes = _service.GetAll().Where(n => n.DogProfileId == dogProfileId)
                .Select(n => new
                {
                    n.NoteId,
                    n.Description,
                    n.Tag,
                    n.Date,
                    UserName = n.User?.Username ?? "Onbekend"
                }).ToList();
            return Ok(notes);
        }
    }
}
