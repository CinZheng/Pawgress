using Pawgress.Data;
using Pawgress.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pawgress.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ApplicationDbContext _context;

        public QuizRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Quiz> GetAll()
        {
            return _context.Quizzes.ToList();
        }

        public Quiz? GetById(Guid id)
        {
            return _context.Quizzes.FirstOrDefault(q => q.Id == id);
        }

        public Quiz Create(Quiz quiz)
        {
            _context.Quizzes.Add(quiz);
            _context.SaveChanges();
            return quiz;
        }

        public Quiz? Update(Guid id, Quiz quiz)
        {
            var existingQuiz = _context.Quizzes.FirstOrDefault(q => q.Id == id);
            if (existingQuiz == null) return null;

            existingQuiz.Name = quiz.Name;
            existingQuiz.Description = quiz.Description;
            existingQuiz.UpdateDate = DateTime.Now;

            _context.SaveChanges();
            return existingQuiz;
        }

        public bool Delete(Guid id)
        {
            var quiz = _context.Quizzes.FirstOrDefault(q => q.Id == id);
            if (quiz == null) return false;

            _context.Quizzes.Remove(quiz);
            _context.SaveChanges();
            return true;
        }
    }
} 