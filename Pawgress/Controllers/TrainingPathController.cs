using Microsoft.AspNetCore.Mvc;
using Pawgress.Dtos;
using Pawgress.Models;
using Pawgress.Services;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingPathController : Controller
    {
        private readonly TrainingPathService _service;

        public TrainingPathController(TrainingPathService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var trainingPaths = _service.GetAll();
            var dtos = trainingPaths.Select(tp => new TrainingPathDto
            {
                TrainingPathId = tp.TrainingPathId,
                Name = tp.Name,
                Description = tp.Description,
                Lessons = tp.Lessons?.Select(l => new LessonDto
                {
                    LessonId = l.LessonId,
                    Name = l.Name,
                    SortOrder = l.SortOrder
                }).ToList(),
                Quizzes = tp.Quizzes?.Select(q => new QuizDto
                {
                    QuizId = q.QuizId,
                    QuizName = q.QuizName,
                    SortOrder = q.SortOrder
                }).ToList(),
                Users = tp.Users?.Select(u => new UserTrainingPathDto
                {
                    UserId = u.UserId,
                    TrainingPathId = u.TrainingPathId,
                    Progress = u.Progress,
                    Status = u.Status,
                    StartDate = u.StartDate,
                    CompletionDate = u.CompletionDate
                }).ToList()
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var trainingPath = _service.GetById(id);
            if (trainingPath == null) return NotFound("Niet gevonden.");

            var dto = new TrainingPathDto
            {
                TrainingPathId = trainingPath.TrainingPathId,
                Name = trainingPath.Name,
                Description = trainingPath.Description,
                Lessons = trainingPath.Lessons?.Select(l => new LessonDto
                {
                    LessonId = l.LessonId,
                    Name = l.Name,
                    SortOrder = l.SortOrder
                }).ToList(),
                Quizzes = trainingPath.Quizzes?.Select(q => new QuizDto
                {
                    QuizId = q.QuizId,
                    QuizName = q.QuizName,
                    SortOrder = q.SortOrder
                }).ToList(),
                Users = trainingPath.Users?.Select(u => new UserTrainingPathDto
                {
                    UserId = u.UserId,
                    TrainingPathId = u.TrainingPathId,
                    Progress = u.Progress,
                    Status = u.Status,
                    StartDate = u.StartDate,
                    CompletionDate = u.CompletionDate
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TrainingPathDto trainingPathDto)
        {
            var trainingPath = new TrainingPath
            {
                TrainingPathId = Guid.NewGuid(),
                Name = trainingPathDto.Name,
                Description = trainingPathDto.Description
            };

            var created = _service.Create(trainingPath);
            return Ok(new TrainingPathDto
            {
                TrainingPathId = created.TrainingPathId,
                Name = created.Name,
                Description = created.Description
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] TrainingPathDto trainingPathDto)
        {
            var trainingPath = _service.GetById(id);
            if (trainingPath == null) return NotFound("Niet gevonden.");

            trainingPath.Name = trainingPathDto.Name;
            trainingPath.Description = trainingPathDto.Description;

            _service.Update(id, trainingPath);
            return Ok(trainingPathDto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return _service.Delete(id) ? Ok("Succesvol verwijderd.") : NotFound("Niet gevonden.");
        }

        [HttpPost("{trainingPathId}/add-lesson")]
        public IActionResult AddLesson(Guid trainingPathId, [FromBody] LessonDto lessonDto)
        {
            try
            {
                var lesson = new Lesson
                {
                    LessonId = lessonDto.LessonId,
                    Name = lessonDto.Name,
                    SortOrder = lessonDto.SortOrder,
                    TrainingPathId = trainingPathId
                };

                _service.AddLesson(trainingPathId, lesson);
                return Ok("Les succesvol toegevoegd.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{trainingPathId}/add-quiz")]
        public IActionResult AddQuiz(Guid trainingPathId, [FromBody] QuizDto quizDto)
        {
            try
            {
                var quiz = new Quiz
                {
                    QuizId = quizDto.QuizId ?? Guid.NewGuid(),
                    QuizName = quizDto.QuizName,
                    QuizDescription = quizDto.QuizDescription,
                    TrainingPathId = trainingPathId,
                    SortOrder = quizDto.SortOrder,
                    QuizQuestions = quizDto.QuizQuestions?.Select(q => new QuizQuestion
                    {
                        QuizQuestionId = q.QuizQuestionId ?? Guid.NewGuid(),
                        QuestionText = q.QuestionText,
                        CorrectAnswer = q.CorrectAnswer,
                        MediaUrl = q.MediaUrl
                    }).ToList()
                };

                _service.AddQuiz(trainingPathId, quiz);
                return Ok("Quiz succesvol toegevoegd.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
