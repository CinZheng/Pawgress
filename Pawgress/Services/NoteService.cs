using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class NoteService : BaseService<Note>
    {
        private readonly ApplicationDbContext _context;

        public NoteService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public User? GetUserById(Guid userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId);
        }

        public override Note? GetById(Guid id)
        {
            return _context.Notes
                .Include(n => n.User) // Eager loading van de User
                .FirstOrDefault(n => n.NoteId == id);
        }

        public override List<Note> GetAll()
        {
            return _context.Notes
                .Include(n => n.User) // Eager loading van de User
                .ToList();
        }
    }
}
