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
            try
            {
                path.UpdateDate = DateTime.Now;
                path.CreationDate = DateTime.Now;

                // Store the TrainingPathItems temporarily
                var items = path.TrainingPathItems?.ToList();
                path.TrainingPathItems = null;

                // First add the TrainingPath
                _context.TrainingPaths.Add(path);
                _context.SaveChanges();

                // Then add the TrainingPathItems if any
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        item.TrainingPathId = path.TrainingPathId;
                    }
                    _context.TrainingPathItemOrders.AddRange(items);
                    _context.SaveChanges();
                }

                // Reload the path with its items before returning
                return _context.TrainingPaths
                    .Include(tp => tp.TrainingPathItems)
                        .ThenInclude(tpi => tpi.TrainingPathItem)
                    .Include(tp => tp.Users)
                    .FirstOrDefault(tp => tp.TrainingPathId == path.TrainingPathId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating training path: {ex.Message}");
            }
        }

        public TrainingPath? Update(Guid id, TrainingPath updatedPath)
        {
            try
            {
                var path = GetById(id);
                if (path == null) return null;

                path.Name = updatedPath.Name;
                path.Description = updatedPath.Description;
                path.UpdateDate = DateTime.Now;

                // Remove existing items
                var existingItems = _context.TrainingPathItemOrders.Where(tpi => tpi.TrainingPathId == id);
                _context.TrainingPathItemOrders.RemoveRange(existingItems);

                // Add new items
                if (updatedPath.TrainingPathItems != null)
                {
                    foreach (var item in updatedPath.TrainingPathItems)
                    {
                        item.TrainingPathId = id;
                        _context.TrainingPathItemOrders.Add(item);
                    }
                }

                _context.SaveChanges();

                // Reload the path with its items before returning
                return _context.TrainingPaths
                    .Include(tp => tp.TrainingPathItems)
                        .ThenInclude(tpi => tpi.TrainingPathItem)
                    .Include(tp => tp.Users)
                    .FirstOrDefault(tp => tp.TrainingPathId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating training path: {ex.Message}");
            }
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
