using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class QuizService : BaseService<Quiz>
    {
        public QuizService(ApplicationDbContext context) : base(context)
        {
        }

        public Quiz AddQuestion(Guid quizId, QuizQuestion question)
        {
            var quiz = GetById(quizId);
            if (quiz == null) throw new Exception("Quiz niet gevonden.");

            quiz.QuizQuestions.Add(question);
            Update(quizId, quiz);
            return quiz;
        }

        public override Quiz? GetById(Guid id)
        {
            // Laadt ook de gerelateerde QuizQuestions
            return _context.Quizzes
                .Include(q => q.QuizQuestions)
                .FirstOrDefault(q => q.QuizId == id);
        }

        public override List<Quiz> GetAll()
        {
            // Laadt ook de gerelateerde QuizQuestions
            return _context.Quizzes
                .Include(q => q.QuizQuestions)
                .ToList();
        }
    }
}
