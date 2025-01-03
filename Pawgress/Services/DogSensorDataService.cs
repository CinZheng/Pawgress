using Pawgress.Data;
using Pawgress.Models;
using System.Threading.Tasks;

namespace Pawgress.Services
{
    public class DogSensorDataService
    {
        private readonly ApplicationDbContext _context;

        public DogSensorDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddDogSensorDataAsync(DogSensorData sensorData)
        {
            _context.DogSensorData.Add(sensorData);
            await _context.SaveChangesAsync();
        }
    }
}
