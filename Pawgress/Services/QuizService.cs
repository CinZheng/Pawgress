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
    }
}
