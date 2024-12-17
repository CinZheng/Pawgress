using Pawgress.Data;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class TrainingPathService : BaseService<TrainingPath>
    {
        public TrainingPathService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
