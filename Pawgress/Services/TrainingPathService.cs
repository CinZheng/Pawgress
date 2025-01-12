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
                .Include(tp => tp.Users)
                .Include(tp => tp.TrainingPathItems)
                    .ThenInclude(tpi => tpi.TrainingPathItem)
                .ToList();
        }

        public TrainingPath? GetById(Guid id)
        {
            return _context.TrainingPaths
                .Include(tp => tp.Users)
                .Include(tp => tp.TrainingPathItems)
                    .ThenInclude(tpi => tpi.TrainingPathItem)
                .FirstOrDefault(tp => tp.TrainingPathId == id);
        }

        public TrainingPath Create(TrainingPath path)
        {
            path.TrainingPathId = Guid.NewGuid();
            path.CreationDate = DateTime.Now;
            path.UpdateDate = DateTime.Now;

            if (path.TrainingPathItems != null)
            {
                foreach (var item in path.TrainingPathItems)
                {
                    item.Id = Guid.NewGuid(); // Ensure IDs are generated for the relationship entries
                }
            }

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
            path.UpdateDate = DateTime.Now;

            // Clear existing TrainingPathItems and re-add them to maintain the order
            _context.TrainingPathItemOrders.RemoveRange(
                _context.TrainingPathItemOrders.Where(tpi => tpi.TrainingPathId == id));

            if (updatedPath.TrainingPathItems != null)
            {
                foreach (var item in updatedPath.TrainingPathItems)
                {
                    item.Id = Guid.NewGuid(); // Ensure IDs are set for the relationship entries
                    _context.TrainingPathItemOrders.Add(item);
                }
            }

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

        public void AddTrainingPathItem(Guid trainingPathId, TrainingPathItem item, int order)
        {
            var path = GetById(trainingPathId);
            if (path == null) throw new Exception("TrainingPath not found");

            var trainingPathItemOrder = new TrainingPathItemOrder
            {
                Id = Guid.NewGuid(),
                TrainingPathId = trainingPathId,
                TrainingPathItemId = item.Id,
                Order = order
            };

            _context.TrainingPathItemOrders.Add(trainingPathItemOrder);
            _context.SaveChanges();
        }

        public void RemoveTrainingPathItem(Guid trainingPathId, Guid trainingPathItemId)
        {
            var trainingPathItemOrder = _context.TrainingPathItemOrders
                .FirstOrDefault(tpi => tpi.TrainingPathId == trainingPathId && tpi.TrainingPathItemId == trainingPathItemId);

            if (trainingPathItemOrder == null) throw new Exception("TrainingPathItem not found in TrainingPath");

            _context.TrainingPathItemOrders.Remove(trainingPathItemOrder);
            _context.SaveChanges();
        }
    }
}
