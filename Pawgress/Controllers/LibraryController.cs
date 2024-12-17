using Pawgress.Models;
using Pawgress.Services;

namespace Pawgress.Controllers
{
    public class LibraryController : BaseController<Library>
    {
        public LibraryController(BaseService<Library> service) : base(service)
        {
        }
    }
}
