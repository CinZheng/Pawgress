using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Services;
using System;
using System.Linq;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly NoteService _service;

        public NoteController(NoteService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var notes = _service.GetAll()
                .Select(n => new NoteDto
                {
                    NoteId = n.NoteId,
                    DogProfileId = n.DogProfileId,
                    UserId = n.UserId,
                    Tag = n.Tag,
                    CreationDate = n.CreationDate,
                    UpdateDate = n.UpdateDate,
                    Description = n.Description,
                    UserName = n.User?.Username ?? "Onbekend"
                })
                .ToList();
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var note = _service.GetById(id);
            if (note == null) return NotFound("Notitie niet gevonden.");

            var noteDto = new NoteDto
            {
                NoteId = note.NoteId,
                DogProfileId = note.DogProfileId,
                UserId = note.UserId,
                Tag = note.Tag,
                CreationDate = note.CreationDate,
                UpdateDate = note.UpdateDate,
                Description = note.Description,
                UserName = note.User?.Username ?? "Onbekend"
            };

            return Ok(noteDto);
        }

        [HttpGet("dogprofile/{dogProfileId}")]
        public IActionResult GetNotesByDogProfile(Guid dogProfileId)
        {
            var notes = _service.GetAll()
                .Where(n => n.DogProfileId == dogProfileId)
                .Select(n => new NoteDto
                {
                    NoteId = n.NoteId,
                    DogProfileId = n.DogProfileId,
                    UserId = n.UserId,
                    Tag = n.Tag,
                    CreationDate = n.CreationDate,
                    UpdateDate = n.UpdateDate,
                    Description = n.Description,
                    UserName = n.User?.Username ?? "Onbekend"
                })
                .ToList();
            return Ok(notes);
        }

        [HttpPost]
        public IActionResult Create([FromBody] NoteDto noteDto)
        {
            var note = new Note
            {
                NoteId = Guid.NewGuid(),
                DogProfileId = noteDto.DogProfileId,
                UserId = noteDto.UserId,
                Tag = noteDto.Tag,
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Description = noteDto.Description
            };

            var createdNote = _service.Create(note);

            // Haal de User op voor de juiste userName
            var user = _service.GetUserById(noteDto.UserId);

            return Ok(new NoteDto
            {
                NoteId = createdNote.NoteId,
                DogProfileId = createdNote.DogProfileId,
                UserId = createdNote.UserId,
                Tag = createdNote.Tag,
                CreationDate = createdNote.CreationDate,
                UpdateDate = createdNote.UpdateDate,
                Description = createdNote.Description,
                UserName = user?.Username ?? "Onbekend"
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] NoteDto noteDto)
        {
            var existingNote = _service.GetById(id);
            if (existingNote == null) return NotFound("Notitie niet gevonden.");
            existingNote.Description = noteDto.Description;
            existingNote.Tag = noteDto.Tag;
            existingNote.CreationDate = noteDto.CreationDate;

            var updatedNote = _service.Update(id, existingNote);
            return Ok(new NoteDto
            {
                NoteId = updatedNote.NoteId,
                DogProfileId = updatedNote.DogProfileId,
                UserId = updatedNote.UserId,
                Tag = updatedNote.Tag,
                UpdateDate = DateTime.Now,
                Description = updatedNote.Description,
                UserName = updatedNote.User?.Username ?? "Onbekend"
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var success = _service.Delete(id);
            return success ? Ok("Notitie succesvol verwijderd.") : NotFound("Notitie niet gevonden.");
        }
    }
}


