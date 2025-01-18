using Pawgress.Data;
using Pawgress.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pawgress.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly ApplicationDbContext _context;

        public LessonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Lesson> GetAll()
        {
            return _context.Lessons.ToList();
        }

        public Lesson? GetById(Guid id)
        {
            return _context.Lessons.FirstOrDefault(l => l.Id == id);
        }

        public Lesson Create(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            _context.SaveChanges();
            return lesson;
        }

        public Lesson? Update(Guid id, Lesson lesson)
        {
            var existingLesson = _context.Lessons.FirstOrDefault(l => l.Id == id);
            if (existingLesson == null) return null;

            existingLesson.Name = lesson.Name;
            existingLesson.Text = lesson.Text;
            existingLesson.Video = lesson.Video;
            existingLesson.Image = lesson.Image;
            existingLesson.MediaUrl = lesson.MediaUrl;
            existingLesson.Tag = lesson.Tag;
            existingLesson.MarkdownContent = lesson.MarkdownContent;
            existingLesson.UpdateDate = DateTime.Now;

            _context.SaveChanges();
            return existingLesson;
        }

        public bool Delete(Guid id)
        {
            var lesson = _context.Lessons.FirstOrDefault(l => l.Id == id);
            if (lesson == null) return false;

            _context.Lessons.Remove(lesson);
            _context.SaveChanges();
            return true;
        }
    }
} 