using Pawgress.Models;
using Pawgress.Services;

namespace Pawgress.Controllers
{
    public class LessonController : BaseController<Lesson>
    {
        public LessonController(LessonService service) : base(service)
        {
        }
    }
}
