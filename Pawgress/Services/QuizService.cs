using Pawgress.Repositories;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class QuizService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public Quiz AddQuestion(Guid quizId, QuizQuestion question)
        {
            var quiz = GetById(quizId);
            if (quiz == null) throw new Exception("Quiz niet gevonden.");

            quiz.QuizQuestions.Add(question);
            Update(quizId, quiz);
            return quiz;
        }

        public Quiz? GetById(Guid id)
        {
            return _quizRepository.GetById(id);
        }

        public List<Quiz> GetAll()
        {
            return _quizRepository.GetAll();
        }

        public Quiz Create(Quiz quiz)
        {
            quiz.Id = Guid.NewGuid();
            quiz.CreationDate = DateTime.Now;
            quiz.UpdateDate = DateTime.Now;
            return _quizRepository.Create(quiz);
        }

        public Quiz? Update(Guid id, Quiz quiz)
        {
            return _quizRepository.Update(id, quiz);
        }

        public bool Delete(Guid id)
        {
            return _quizRepository.Delete(id);
        }
    }
}
