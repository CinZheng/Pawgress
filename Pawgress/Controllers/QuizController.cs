using Microsoft.AspNetCore.Mvc;
using Pawgress.Data;
using Pawgress.Dtos;
using Pawgress.Models;
using Pawgress.Services;
using System.Linq;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly QuizService _quizService;
        private readonly ApplicationDbContext _context;

        public QuizController(QuizService service, ApplicationDbContext context)
        {
            _quizService = service;
            _context = context;
        }

          [HttpGet]
        public IActionResult GetAll()
        {
            var quizzes = _quizService.GetAll();
            var dtos = quizzes.Select(q => new QuizDto
            {
                QuizId = q.QuizId,
                QuizName = q.QuizName,
                QuizDescription = q.QuizDescription,
                QuizQuestions = q.QuizQuestions.Select(qq => new QuizQuestionDto
                {
                    QuizQuestionId = qq.QuizQuestionId,
                    QuestionText = qq.QuestionText,
                    CorrectAnswer = qq.CorrectAnswer,
                    MediaUrl = qq.MediaUrl
                }).ToList()
            }).ToList();

            return Ok(dtos);
        }

          [HttpGet("{id}")]
        public IActionResult GetQuizById(Guid id)
        {
            var quiz = _quizService.GetById(id);
            if (quiz == null) return NotFound("Quiz niet gevonden.");

            var quizDto = new QuizDto
            {
                QuizId = quiz.QuizId,
                QuizName = quiz.QuizName,
                QuizDescription = quiz.QuizDescription,
                TrainingPathId = quiz.TrainingPathId,
                QuizQuestions = quiz.QuizQuestions?.Select(q => new QuizQuestionDto
                {
                    QuizQuestionId = q.QuizQuestionId,
                    QuestionText = q.QuestionText,
                    CorrectAnswer = q.CorrectAnswer,
                    MediaUrl = q.MediaUrl
                }).ToList()
            };

            return Ok(quizDto);
        }

         [HttpPost]
        public IActionResult CreateQuiz([FromBody] QuizDto quizDto)
        {
            var quiz = new Quiz
            {
                QuizId = Guid.NewGuid(),
                QuizName = quizDto.QuizName,
                QuizDescription = quizDto.QuizDescription,
                TrainingPathId = quizDto.TrainingPathId,
                QuizQuestions = quizDto.QuizQuestions?.Select(q => new QuizQuestion
                {
                    QuizQuestionId = Guid.NewGuid(),
                    QuestionText = q.QuestionText,
                    CorrectAnswer = q.CorrectAnswer,
                    MediaUrl = q.MediaUrl
                }).ToList()
            };

            var createdQuiz = _quizService.Create(quiz);
            return Ok(new { createdQuiz.QuizId });
        }

         [HttpPut("{id}")]
        public IActionResult UpdateQuiz(Guid id, [FromBody] QuizDto quizDto)
        {
            var existingQuiz = _quizService.GetById(id);
            if (existingQuiz == null) return NotFound("Quiz niet gevonden.");

            existingQuiz.QuizName = quizDto.QuizName;
            existingQuiz.QuizDescription = quizDto.QuizDescription;
            existingQuiz.TrainingPathId = quizDto.TrainingPathId;

            // Update de vragen
            var updatedQuestions = quizDto.QuizQuestions.Select(q => new QuizQuestion
            {
                QuizQuestionId = q.QuizQuestionId ?? Guid.NewGuid(),
                QuestionText = q.QuestionText,
                CorrectAnswer = q.CorrectAnswer,
                MediaUrl = q.MediaUrl,
                QuizId = id
            }).ToList();

            _context.Questions.RemoveRange(existingQuiz.QuizQuestions);
            _context.Questions.AddRange(updatedQuestions);

            _quizService.Update(id, existingQuiz);
            _context.SaveChanges();

            return Ok("Quiz succesvol bijgewerkt.");
        }

        [HttpPost("{quizId}/add-question")]
        public IActionResult AddQuestionToQuiz(Guid quizId, [FromBody] QuizQuestionDto questionDto)
        {
            var quiz = _quizService.GetById(quizId);
            if (quiz == null) return NotFound("Quiz niet gevonden.");

            var question = new QuizQuestion
            {
                QuizQuestionId = Guid.NewGuid(),
                QuestionText = questionDto.QuestionText,
                CorrectAnswer = questionDto.CorrectAnswer,
                MediaUrl = questionDto.MediaUrl,
                QuizId = quizId
            };

            _context.Questions.Add(question);
            _context.SaveChanges();

            return Ok("Vraag succesvol toegevoegd aan quiz.");
        }

        [HttpGet("{quizId}/questions")]
        public IActionResult GetQuestionsByQuiz(Guid quizId)
        {
            var quiz = _quizService.GetById(quizId);
            if (quiz == null) return NotFound("Quiz niet gevonden.");

            var questions = _context.Questions
                .Where(q => q.QuizId == quizId)
                .Select(q => new QuizQuestionDto
                {
                    QuizQuestionId = q.QuizQuestionId,
                    QuestionText = q.QuestionText,
                    CorrectAnswer = q.CorrectAnswer,
                    MediaUrl = q.MediaUrl
                }).ToList();

            return Ok(questions);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteQuiz(Guid id)
        {
            var quiz = _quizService.GetById(id);
            if (quiz == null) return NotFound("Quiz niet gevonden.");

            var questions = _context.Questions.Where(q => q.QuizId == id).ToList();
            _context.Questions.RemoveRange(questions);

            _quizService.Delete(id);

            _context.SaveChanges();

            return Ok("Quiz succesvol verwijderd.");

        }
    }
}
