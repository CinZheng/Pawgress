using Pawgress.Data;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class FolderService : BaseService<Folder>
    {
        public FolderService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
