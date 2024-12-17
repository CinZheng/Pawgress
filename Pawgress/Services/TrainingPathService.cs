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
            return _context.TrainingPaths.ToList();
        }

        public TrainingPath? GetById(Guid id)
        {
            return _context.TrainingPaths.Find(id);
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
            var path = _context.TrainingPaths.Find(id);
            if (path == null) return null;

            _context.Entry(path).CurrentValues.SetValues(updatedPath);
            _context.SaveChanges();
            return path;
        }

        public bool Delete(Guid id)
        {
            var path = _context.TrainingPaths.Find(id);
            if (path == null) return false;

            _context.TrainingPaths.Remove(path);
            _context.SaveChanges();
            return true;
        }
    }
}
