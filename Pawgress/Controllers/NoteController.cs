using Pawgress.Models;
using Pawgress.Services;

namespace Pawgress.Controllers
{
    public class NoteController : BaseController<Note>
    {
        public NoteController(NoteService service) : base(service)
        {
        }
    }
}
