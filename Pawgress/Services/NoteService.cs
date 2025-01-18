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
            _noteRepository = noteRepository;
            _userRepository = userRepository;
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
            var notes = _noteRepository.GetAll();
            foreach (var note in notes)
            {
                note.User = _userRepository.GetById(note.UserId);
            }
            return notes;
        }

        public Note Create(Note note)
        {
            note.NoteId = Guid.NewGuid();
            note.CreationDate = DateTime.Now;
            note.UpdateDate = DateTime.Now;
            return _noteRepository.Create(note);
        }

        public Note? Update(Guid id, Note note)
        {
            return _noteRepository.Update(id, note);
        }

        public bool Delete(Guid id)
        {
            return _noteRepository.Delete(id);
        }
    }
}
