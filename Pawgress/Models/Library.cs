using System;

namespace Pawgress.Models
{

    public class Library
    {
        public Guid LibraryId { get; set; }
        public string Name { get; set; }
        
        //public List<string>? Tags { get; set; } = new List<string>();
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public List<LibraryLesson> LibraryLessons { get; set; } = new List<LibraryLesson>();
        public List<LibraryQuiz> LibraryQuizzes { get; set; } = new List<LibraryQuiz>();
    }
}
