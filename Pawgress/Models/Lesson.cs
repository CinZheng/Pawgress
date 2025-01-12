using System;

namespace Pawgress.Models
{
    public class Lesson : TrainingPathItem
    {
        public string? Name { get; set; }
        public string? Text { get; set; }
        public string? Video { get; set; }
        public string? Image { get; set; }
        public string? MediaUrl { get; set; }
        public string? Tag { get; set; }
        public string? MarkdownContent { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<LibraryLesson> LibraryLessons { get; set; } = new List<LibraryLesson>();

    }
}