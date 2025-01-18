using Xunit;
using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using Pawgress.Models;
using Pawgress.Services;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Pawgress.Tests.Services
{
    public class DogSensorDataServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly DogSensorDataService _service;
        private readonly List<DogSensorData> _sensorData;

        public DogSensorDataServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new DogSensorDataService(_context);
            _sensorData = new List<DogSensorData>();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddDogSensorDataAsync_ShouldAddDataAndSaveChanges()
        {
            // Arrange
            var sensorData = new DogSensorData
            {
                DogSensorDataId = Guid.NewGuid(),
                Name = "Test Sensor",
                Description = "Test Description",
                SensorType = SensorType.Accelerometer,
                Unit = "m/s²",
                AverageValue = 9.81,
                DogProfileId = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            // Act
            await _service.AddDogSensorDataAsync(sensorData);

            // Assert
            var savedData = await _context.Set<DogSensorData>().FirstOrDefaultAsync();
            Assert.NotNull(savedData);
            Assert.Equal(sensorData.Name, savedData.Name);
            Assert.Equal(sensorData.Description, savedData.Description);
            Assert.Equal(sensorData.SensorType, savedData.SensorType);
            Assert.Equal(sensorData.Unit, savedData.Unit);
            Assert.Equal(sensorData.AverageValue, savedData.AverageValue);
        }

        [Fact]
        public async Task AddDogSensorDataAsync_WithNullData_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _service.AddDogSensorDataAsync(null));
        }

        [Fact]
        public async Task AddDogSensorDataAsync_WithInvalidData_ShouldThrowException()
        {
            // Arrange
            var sensorData = new DogSensorData
            {
                DogSensorDataId = Guid.NewGuid(),
                Name = null // Name is required
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => 
                _service.AddDogSensorDataAsync(sensorData));
        }
    }
} 