using Microsoft.EntityFrameworkCore;
using Pawgress.Data;

namespace Pawgress.Services
{
    public class BaseService<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseService(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual T? GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public virtual T Create(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual T? Update(Guid id, T updatedEntity)
        {
            var existingEntity = _dbSet.Find(id);
            if (existingEntity == null) return null;

            // voorkomt wijzigingen aan de primary key
            foreach (var property in _context.Entry(updatedEntity).Properties)
            {
                if (property.Metadata.IsPrimaryKey())
                {
                    continue; // sla primary key properties over
                }
                _context.Entry(existingEntity).Property(property.Metadata.Name).CurrentValue = property.CurrentValue;
            }

            // markeer de entity als gewijzigd
            _context.Entry(existingEntity).State = EntityState.Modified;

            _context.SaveChanges();
            return existingEntity;
        }

        public virtual bool Delete(Guid id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            _context.SaveChanges();
            return true;
        }
    }
}
