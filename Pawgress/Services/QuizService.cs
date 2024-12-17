using Pawgress.Data;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class QuizService : BaseService<Quiz>
    {
        public QuizService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
