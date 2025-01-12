using System;
using System.Collections.Generic;

namespace Pawgress.Models
{
    public class Folder
    {
        public Guid FolderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentFolderId { get; set; }
        public List<Folder> SubFolders { get; set; } = new List<Folder>();
        public List<Lesson> Lessons { get; set; } = new List<Lesson>();
        public List<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public Folder? ParentFolder { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}

