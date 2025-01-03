using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Services;
using System;
using System.Threading.Tasks;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DogSensorDataController : ControllerBase
    {
        private readonly DogSensorDataService _service;

        public DogSensorDataController(DogSensorDataService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DogSensorData dogSensorData)
        {
            if (dogSensorData == null || dogSensorData.DogProfileId == Guid.Empty)
            {
                return BadRequest("Ongeldige gegevens. DogProfileId is vereist.");
            }

            await _service.AddDogSensorDataAsync(dogSensorData);
            return Ok("Sensor data succesvol opgeslagen.");
        }
    }
}
