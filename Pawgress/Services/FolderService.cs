using Pawgress.Data;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class FolderService
    {
        private readonly ApplicationDbContext _context;

        public FolderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int CountFolders()
        {
            return _context.Folders.Count();
        }

        public List<Folder> GetAllFolders()
        {
            return _context.Folders.ToList();
        }

        public Folder? GetById(Guid id)
        {
            return _context.Folders.Find(id);
        }
    }
}
