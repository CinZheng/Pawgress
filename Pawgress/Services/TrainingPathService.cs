using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class TrainingPathService
    {
        private readonly ApplicationDbContext _context;

        public TrainingPathService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<TrainingPath> GetAll()
        {
            return _context.TrainingPaths
                .Include(tp => tp.Lessons.OrderBy(l => l.SortOrder))
                .Include(tp => tp.Quizzes.OrderBy(q => q.SortOrder))
                .ToList();
        }

        public TrainingPath? GetById(Guid id)
        {
            return _context.TrainingPaths
                .Include(tp => tp.Lessons.OrderBy(l => l.SortOrder))
                .Include(tp => tp.Quizzes.OrderBy(q => q.SortOrder))
                .Include(tp => tp.Users)
                .FirstOrDefault(tp => tp.TrainingPathId == id);
        }

        public TrainingPath Create(TrainingPath path)
        {
            path.TrainingPathId = Guid.NewGuid();
            _context.TrainingPaths.Add(path);
            _context.SaveChanges();
            return path;
        }

        public TrainingPath? Update(Guid id, TrainingPath updatedPath)
        {
            var path = GetById(id);
            if (path == null) return null;

            path.Name = updatedPath.Name;
            path.Description = updatedPath.Description;
            _context.SaveChanges();
            return path;
        }

        public bool Delete(Guid id)
        {
            var path = GetById(id);
            if (path == null) return false;

            _context.TrainingPaths.Remove(path);
            _context.SaveChanges();
            return true;
        }

        public void AddLesson(Guid trainingPathId, Lesson lesson)
        {
            var path = GetById(trainingPathId);
            if (path == null) throw new Exception("TrainingPath not found");

            lesson.SortOrder = path.Lessons.Count + 1;
            path.Lessons.Add(lesson);
            _context.SaveChanges();
        }

        public void AddQuiz(Guid trainingPathId, Quiz quiz)
        {
            var path = GetById(trainingPathId);
            if (path == null) throw new Exception("TrainingPath not found");

            quiz.SortOrder = path.Quizzes.Count + 1;
            path.Quizzes.Add(quiz);
            _context.SaveChanges();
        }
    }
}
