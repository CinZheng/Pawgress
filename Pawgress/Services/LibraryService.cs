using Pawgress.Data;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class LibraryService : BaseService<Library>
    {
        public LibraryService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
