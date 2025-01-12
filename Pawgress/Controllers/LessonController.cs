using Microsoft.AspNetCore.Mvc;
using Pawgress.Dtos;
using Pawgress.Models;
using Pawgress.Services;
using System;
using System.Linq;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly LessonService _service;

        public LessonController(LessonService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var lessons = _service.GetAll();
            var dtos = lessons.Select(ToLessonDto).ToList();
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var lesson = _service.GetById(id);
            if (lesson == null)
                return NotFound($"Lesson with ID {id} not found.");

            return Ok(ToLessonDto(lesson));
        }

        [HttpPost]
        public IActionResult Create([FromBody] LessonDto lessonDto)
        {
            if (string.IsNullOrWhiteSpace(lessonDto.Name))
                return BadRequest("Lesson name is required.");

            var lesson = new Lesson
            {
                Id = Guid.NewGuid(),
                Name = lessonDto.Name,
                Text = lessonDto.Text,
                Video = lessonDto.Video,
                Image = lessonDto.Image,
                MediaUrl = lessonDto.MediaUrl,
                Tag = lessonDto.Tag,
                MarkdownContent = lessonDto.MarkdownContent,
                TrainingPathId = lessonDto.TrainingPathId,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            var created = _service.Create(lesson);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToLessonDto(created));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] LessonDto lessonDto)
        {
            var lesson = _service.GetById(id);
            if (lesson == null)
                return NotFound($"Lesson with ID {id} not found.");

            lesson.Name = lessonDto.Name;
            lesson.Text = lessonDto.Text;
            lesson.Video = lessonDto.Video;
            lesson.Image = lessonDto.Image;
            lesson.MediaUrl = lessonDto.MediaUrl;
            lesson.Tag = lessonDto.Tag;
            lesson.MarkdownContent = lessonDto.MarkdownContent;
            lesson.TrainingPathId = lessonDto.TrainingPathId;
            lesson.UpdateDate = DateTime.UtcNow;

            _service.Update(id, lesson);

            return Ok(ToLessonDto(lesson));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var success = _service.Delete(id);
            if (!success)
                return NotFound($"Lesson with ID {id} not found.");

            return Ok("Lesson successfully deleted.");
        }

        // Helper Method to Map Lesson to LessonDto
        private static LessonDto ToLessonDto(Lesson lesson)
        {
            return new LessonDto
            {
                Id = lesson.Id,
                Name = lesson.Name,
                Text = lesson.Text,
                Video = lesson.Video,
                Image = lesson.Image,
                MediaUrl = lesson.MediaUrl,
                Tag = lesson.Tag,
                MarkdownContent = lesson.MarkdownContent,
                TrainingPathId = lesson.TrainingPathId,
                CreationDate = lesson.CreationDate,
                UpdateDate = lesson.UpdateDate
            };
        }
    }
}
