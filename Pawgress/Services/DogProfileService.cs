using Pawgress.Data;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class DogProfileService : BaseService<DogProfile>
    {
        public DogProfileService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
