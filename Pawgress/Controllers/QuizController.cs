using Microsoft.AspNetCore.Mvc;
using Pawgress.Data;
using Pawgress.Dtos;
using Pawgress.Models;
using Pawgress.Services;
using System;
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
                Id = q.Id, // Polymorphic Id
                QuizName = q.Name,
                QuizDescription = q.Description,
                CreationDate = q.CreationDate,
                UpdateDate = q.UpdateDate,
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
                Id = quiz.Id, // Polymorphic Id
                QuizName = quiz.Name,
                QuizDescription = quiz.Description,
                TrainingPathId = quiz.TrainingPathId,
                CreationDate = quiz.CreationDate,
                UpdateDate = quiz.UpdateDate,
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
                Id = Guid.NewGuid(), // Polymorphic Id
                Name = quizDto.QuizName,
                Description = quizDto.QuizDescription,
                TrainingPathId = quizDto.TrainingPathId,
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                QuizQuestions = quizDto.QuizQuestions?.Select(q => new QuizQuestion
                {
                    QuizQuestionId = Guid.NewGuid(),
                    QuestionText = q.QuestionText,
                    CorrectAnswer = q.CorrectAnswer,
                    MediaUrl = q.MediaUrl
                }).ToList()
            };

            var createdQuiz = _quizService.Create(quiz);
            return Ok(new { createdQuiz.Id });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateQuiz(Guid id, [FromBody] QuizDto quizDto)
        {
            var existingQuiz = _quizService.GetById(id);
            if (existingQuiz == null) return NotFound("Quiz niet gevonden.");

            existingQuiz.Name = quizDto.QuizName;
            existingQuiz.Description = quizDto.QuizDescription;
            existingQuiz.TrainingPathId = quizDto.TrainingPathId;
            existingQuiz.UpdateDate = DateTime.Now;

            // Update questions
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
                QuizId = quizId,
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now
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
                    MediaUrl = q.MediaUrl,
                    CreationDate = q.CreationDate,
                    UpdateDate = q.UpdateDate
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
