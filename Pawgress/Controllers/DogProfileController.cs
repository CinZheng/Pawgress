using Pawgress.Models;
using Pawgress.Services;

namespace Pawgress.Controllers
{
    public class DogProfileController : BaseController<DogProfile>
    {
        public DogProfileController(DogProfileService service) : base(service)
        {
        }
    }
}
