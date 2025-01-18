using Pawgress.Repositories;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class LessonService
    {
        private readonly ILessonRepository _lessonRepository;

        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public List<Lesson> GetAll()
        {
            return _lessonRepository.GetAll();
        }

        public Lesson? GetById(Guid id)
        {
            return _lessonRepository.GetById(id);
        }

        public Lesson Create(Lesson lesson)
        {
            lesson.Id = Guid.NewGuid();
            lesson.CreationDate = DateTime.Now;
            lesson.UpdateDate = DateTime.Now;
            return _lessonRepository.Create(lesson);
        }

        public Lesson? Update(Guid id, Lesson lesson)
        {
            return _lessonRepository.Update(id, lesson);
        }

        public bool Delete(Guid id)
        {
            return _lessonRepository.Delete(id);
        }
    }
}
