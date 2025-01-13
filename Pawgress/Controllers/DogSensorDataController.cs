using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pawgress.Models;
using Pawgress.Models.Dtos;
using Pawgress.Services;
using Pawgress.Data;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DogSensorDataController : ControllerBase
    {
        private readonly DogSensorDataService _service;
        private readonly ApplicationDbContext _context;

        public DogSensorDataController(DogSensorDataService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet("dog/{dogProfileId}")]
        public async Task<IActionResult> GetByDogProfile(Guid dogProfileId)
        {
            try
            {
                var sensorData = await _context.DogSensorData
                    .Where(d => d.DogProfileId == dogProfileId)
                    .OrderByDescending(d => d.CreationDate)
                    .ToListAsync();

                return Ok(sensorData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Er is een fout opgetreden: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDogSensorDataDto dto)
        {
            if (dto == null || dto.DogProfileId == Guid.Empty)
            {
                return BadRequest("Ongeldige gegevens. DogProfileId is vereist.");
            }

            try
            {
                var dogSensorData = new DogSensorData
                {
                    DogSensorDataId = Guid.NewGuid(),
                    Name = dto.Name,
                    Description = dto.Description,
                    SensorType = dto.SensorType,
                    Unit = dto.Unit,
                    AverageValue = dto.AverageValue,
                    DogProfileId = dto.DogProfileId,
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                await _service.AddDogSensorDataAsync(dogSensorData);
                return Ok(dogSensorData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Er is een fout opgetreden: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var sensorData = await _context.DogSensorData.FindAsync(id);
                if (sensorData == null)
                {
                    return NotFound("Sensordata niet gevonden.");
                }

                _context.DogSensorData.Remove(sensorData);
                await _context.SaveChangesAsync();

                return Ok("Sensordata succesvol verwijderd.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Er is een fout opgetreden: {ex.Message}");
            }
        }
    }
}
