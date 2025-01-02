using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Services;
using Pawgress.Dtos;
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
            var dtos = lessons.Select(lesson => new LessonDto
            {
                LessonId = lesson.LessonId,
                Name = lesson.Name,
                Text = lesson.Text,
                Video = lesson.Video,
                Image = lesson.Image,
                MediaUrl = lesson.MediaUrl,
                Tag = lesson.Tag,
                MarkdownContent = lesson.MarkdownContent,
                TrainingPathId = lesson.TrainingPathId
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var lesson = _service.GetById(id);
            if (lesson == null) return NotFound();

            var dto = new LessonDto
            {
                LessonId = lesson.LessonId,
                Name = lesson.Name,
                Text = lesson.Text,
                Video = lesson.Video,
                Image = lesson.Image,
                MediaUrl = lesson.MediaUrl,
                Tag = lesson.Tag,
                MarkdownContent = lesson.MarkdownContent,
                TrainingPathId = lesson.TrainingPathId
            };
            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Create([FromBody] LessonDto lessonDto)
        {
            var lesson = new Lesson
            {
                LessonId = Guid.NewGuid(),
                Name = lessonDto.Name,
                Text = lessonDto.Text,
                Video = lessonDto.Video,
                Image = lessonDto.Image,
                MediaUrl = lessonDto.MediaUrl,
                Tag = lessonDto.Tag,
                MarkdownContent = lessonDto.MarkdownContent,
                TrainingPathId = lessonDto.TrainingPathId
            };

            var created = _service.Create(lesson);
            return Ok(new LessonDto
            {
                LessonId = created.LessonId,
                Name = created.Name,
                Text = created.Text,
                Video = created.Video,
                Image = created.Image,
                MediaUrl = created.MediaUrl,
                Tag = created.Tag,
                MarkdownContent = created.MarkdownContent,
                TrainingPathId = created.TrainingPathId
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] LessonDto lessonDto)
        {
            var lesson = _service.GetById(id);
            if (lesson == null) return NotFound();

            lesson.Name = lessonDto.Name;
            lesson.Text = lessonDto.Text;
            lesson.Video = lessonDto.Video;
            lesson.Image = lessonDto.Image;
            lesson.MediaUrl = lessonDto.MediaUrl;
            lesson.Tag = lessonDto.Tag;
            lesson.MarkdownContent = lessonDto.MarkdownContent;
            lesson.TrainingPathId = lessonDto.TrainingPathId;

            _service.Update(id, lesson);
            return Ok(lessonDto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var success = _service.Delete(id);
            if (!success) return NotFound();

            return Ok("Les succesvol verwijderd.");
        }
    }
}
