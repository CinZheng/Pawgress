using Pawgress.Data;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class LessonService : BaseService<Lesson>
    {
        public LessonService(ApplicationDbContext context) : base(context) { }
    }
}
