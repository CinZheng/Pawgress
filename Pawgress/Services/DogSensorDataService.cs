using Pawgress.Data;
using Pawgress.Models;
using System;
using System.Threading.Tasks;

namespace Pawgress.Services
{
    public class DogSensorDataService
    {
        private readonly ApplicationDbContext _context;

        public DogSensorDataService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddDogSensorDataAsync(DogSensorData sensorData)
        {
            if (sensorData == null)
                throw new ArgumentNullException(nameof(sensorData));

            _context.Set<DogSensorData>().Add(sensorData);
            await _context.SaveChangesAsync();
        }
    }
}
