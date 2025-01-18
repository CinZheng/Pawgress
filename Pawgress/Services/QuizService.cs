using Pawgress.Repositories;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class QuizService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository ?? throw new ArgumentNullException(nameof(quizRepository));
        }

        public Quiz AddQuestion(Guid quizId, QuizQuestion question)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));

            var quiz = GetById(quizId);
            if (quiz == null) throw new Exception("Quiz niet gevonden.");

            quiz.QuizQuestions.Add(question);
            quiz.UpdateDate = DateTime.UtcNow;
            Update(quizId, quiz);
            return quiz;
        }

        public Quiz? GetById(Guid id)
        {
            return _quizRepository.GetById(id);
        }

        public List<Quiz> GetAll()
        {
            return _quizRepository.GetAll() ?? new List<Quiz>();
        }

        public Quiz Create(Quiz quiz)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));

            quiz.Id = Guid.NewGuid();
            quiz.CreationDate = DateTime.UtcNow;
            quiz.UpdateDate = DateTime.UtcNow;
            return _quizRepository.Create(quiz);
        }

        public Quiz? Update(Guid id, Quiz quiz)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));

            var existingQuiz = GetById(id);
            if (existingQuiz == null) return null;

            quiz.CreationDate = existingQuiz.CreationDate;
            quiz.UpdateDate = DateTime.UtcNow;
            return _quizRepository.Update(id, quiz);
        }

        public bool Delete(Guid id)
        {
            return _quizRepository.Delete(id);
        }
    }
}
