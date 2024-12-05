using System;

namespace Pawgress.Models
{

    public class Library
    {
        public Guid LibraryId { get; set; }
        public string Name { get; set; }
        public List<Lesson>? Lessons { get; set; } = new List<Lesson>();
        public List<Quiz>? Quizzes { get; set; } = new List<Quiz>();
        //public List<string>? Tags { get; set; } = new List<string>();
    }
}
