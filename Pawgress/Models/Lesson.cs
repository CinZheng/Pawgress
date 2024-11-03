using System;
using System.Collections.Generic;

namespace Pawgress.Models
{
    public class Lesson
    {
        public Guid LessonId { get; set; }
        public Guid TrainingPathId { get; set; }
        public string LessonName { get; set; }
        public List<Page> Pages { get; set; } = new List<Page>();

        public TrainingPath TrainingPath { get; set; }
    }
}
