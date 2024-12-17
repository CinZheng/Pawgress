using Pawgress.Data;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class NoteService
    {
        private readonly ApplicationDbContext _context;

        public NoteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Note> GetNotesByUser(Guid userId)
        {
            return _context.Notes.Where(n => n.UserId == userId).ToList();
        }

        public Note? GetById(Guid id)
        {
            return _context.Notes.Find(id);
        }

        public Note Create(Note note)
        {
            note.NoteId = Guid.NewGuid();
            _context.Notes.Add(note);
            _context.SaveChanges();
            return note;
        }

        public bool Delete(Guid id)
        {
            var note = _context.Notes.Find(id);
            if (note == null) return false;

            _context.Notes.Remove(note);
            _context.SaveChanges();
            return true;
        }
    }
}
