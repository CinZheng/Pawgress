using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Services;

namespace Pawgress.Controllers
{
    public class LessonController : BaseController<Lesson>
    {
        private readonly LessonService _lessonService;

        public LessonController(LessonService service) : base(service)
        {
            _lessonService = service;
        }

        /*[HttpPost("add-markdown")]
        public IActionResult CreateLessonWithMarkdown([FromBody] Lesson lesson)
        {
            var createdLesson = _lessonService.Create(lesson);
            return Ok(createdLesson);
        }*/
    }
}
