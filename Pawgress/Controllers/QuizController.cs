using Microsoft.AspNetCore.Mvc;
using Pawgress.Data;
using Pawgress.Models;
using Pawgress.Services;
using System.Linq;

namespace Pawgress.Controllers
{
    public class QuizController : BaseController<Quiz>
    {
        private readonly QuizService _quizService;
        private readonly ApplicationDbContext _context;

        public QuizController(QuizService service, ApplicationDbContext context) : base(service)
        {
            _quizService = service;
            _context = context;
        }

        [HttpPost("add-question")]
        public IActionResult AddQuestionToQuiz(Guid quizId, [FromBody] QuizQuestion question)
        {
            // check of quiz bestaat
            var quiz = _quizService.GetById(quizId);
            if (quiz == null) return NotFound("Quiz niet gevonden.");

            // koppel de vraag aan de quiz
            question.QuizId = quizId;

            // voeg vraag aan db
            _context.Questions.Add(question);
            _context.SaveChanges();

            return Ok("Vraag succesvol toegevoegd aan quiz.");
        }

        // endpoint om alle vragen van een specifieke quiz op te halen, handig voor testen
        [HttpGet("{quizId}/questions")]
        public IActionResult GetQuestionsByQuiz(Guid quizId)
        {
            var quiz = _quizService.GetById(quizId);
            if (quiz == null) return NotFound("Quiz niet gevonden.");

            var questions = _context.Questions.Where(q => q.QuizId == quizId).ToList();

            return Ok(questions);
        }
    }
}