using Pawgress.Repositories;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class NoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IUserRepository _userRepository;

        public NoteService(INoteRepository noteRepository, IUserRepository userRepository)
        {
            _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public User? GetUserById(Guid userId)
        {
            return _userRepository.GetById(userId);
        }

        public Note? GetById(Guid id)
        {
            return _noteRepository.GetById(id);
        }

        public List<Note> GetAll()
        {
            var notes = _noteRepository.GetAll() ?? new List<Note>();
            foreach (var note in notes)
            {
                note.User = _userRepository.GetById(note.UserId);
            }
            return notes;
        }

        public Note Create(Note note)
        {
            if (note == null) throw new ArgumentNullException(nameof(note));

            note.NoteId = Guid.NewGuid();
            note.CreationDate = DateTime.UtcNow;
            note.UpdateDate = DateTime.UtcNow;
            return _noteRepository.Create(note);
        }

        public Note? Update(Guid id, Note note)
        {
            if (note == null) throw new ArgumentNullException(nameof(note));

            var existingNote = GetById(id);
            if (existingNote == null) return null;

            note.CreationDate = existingNote.CreationDate;
            note.UpdateDate = DateTime.UtcNow;
            return _noteRepository.Update(id, note);
        }

        public bool Delete(Guid id)
        {
            return _noteRepository.Delete(id);
        }
    }
}
