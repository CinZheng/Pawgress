using System;

namespace Pawgress.Models
{
    public class Lesson
    {
        public Guid LessonId { get; set; }
        public string Name { get; set; }
        public string? Text { get; set; }
        public string? Video { get; set; }
        public string? Image { get; set; }
        public string? MediaUrl { get; set; }
        public string? Tag { get; set; }
        public string? MarkdownContent { get; set; }
        public Guid? TrainingPathId { get; set; }
        public TrainingPath? TrainingPath { get; set; }

    }
}
