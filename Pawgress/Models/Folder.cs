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
        public List<Page> Pages { get; set; } = new List<Page>();
        public Folder ParentFolder { get; set; }
    }
}

