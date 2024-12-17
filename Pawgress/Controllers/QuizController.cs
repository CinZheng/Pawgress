using Pawgress.Models;
using Pawgress.Services;

namespace Pawgress.Controllers
{
    public class QuizController : BaseController<Quiz>
    {
        public QuizController(BaseService<Quiz> service) : base(service)
        {
        }
    }
}
