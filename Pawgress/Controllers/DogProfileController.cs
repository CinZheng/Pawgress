using Pawgress.Models;
using Pawgress.Services;

namespace Pawgress.Controllers
{
    public class DogProfileController : BaseController<DogProfile>
    {
        public DogProfileController(BaseService<DogProfile> service) : base(service)
        {
        }
    }
}
