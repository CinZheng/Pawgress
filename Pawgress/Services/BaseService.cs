using Microsoft.EntityFrameworkCore;
using Pawgress.Data;

namespace Pawgress.Services
{
    public class BaseService<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseService(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T? GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public T Create(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public T? Update(Guid id, T updatedEntity)
        {
            var existingEntity = _dbSet.Find(id);
            if (existingEntity == null) return null;

            // Zorg dat EF Core weet dat de entiteit is gewijzigd
            _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            _context.Entry(existingEntity).State = EntityState.Modified;

            // Opslaan van wijzigingen
            _context.SaveChanges();
            return existingEntity;
        }

        public bool Delete(Guid id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            _context.SaveChanges();
            return true;
        }
    }
}
