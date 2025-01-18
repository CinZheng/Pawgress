using Pawgress.Repositories;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class LessonService
    {
        private readonly ILessonRepository _lessonRepository;

        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository ?? throw new ArgumentNullException(nameof(lessonRepository));
        }

        public List<Lesson> GetAll()
        {
            return _lessonRepository.GetAll() ?? new List<Lesson>();
        }

        public Lesson? GetById(Guid id)
        {
            return _lessonRepository.GetById(id);
        }

        public Lesson Create(Lesson lesson)
        {
            if (lesson == null) throw new ArgumentNullException(nameof(lesson));

            lesson.Id = Guid.NewGuid();
            lesson.CreationDate = DateTime.UtcNow;
            lesson.UpdateDate = DateTime.UtcNow;
            return _lessonRepository.Create(lesson);
        }

        public Lesson? Update(Guid id, Lesson lesson)
        {
            if (lesson == null) throw new ArgumentNullException(nameof(lesson));

            var existingLesson = GetById(id);
            if (existingLesson == null) return null;

            lesson.CreationDate = existingLesson.CreationDate;
            lesson.UpdateDate = DateTime.UtcNow;
            return _lessonRepository.Update(id, lesson);
        }

        public bool Delete(Guid id)
        {
            return _lessonRepository.Delete(id);
        }
    }
}
