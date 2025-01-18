using Xunit;
using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using Pawgress.Models;
using Pawgress.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pawgress.Tests.Services
{
    public class TrainingPathServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly TrainingPathService _service;
        private readonly List<TrainingPath> _trainingPaths;
        private readonly List<TrainingPathItemOrder> _trainingPathItems;

        public TrainingPathServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new TrainingPathService(_context);
            
            _trainingPaths = new List<TrainingPath>
            {
                new TrainingPath { 
                    TrainingPathId = Guid.NewGuid(), 
                    Name = "Path1", 
                    Description = "Description1",
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                },
                new TrainingPath { 
                    TrainingPathId = Guid.NewGuid(), 
                    Name = "Path2", 
                    Description = "Description2",
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                }
            };

            _trainingPathItems = new List<TrainingPathItemOrder>();

            _context.Set<TrainingPath>().AddRange(_trainingPaths);
            _context.Set<TrainingPathItemOrder>().AddRange(_trainingPathItems);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void GetAll_ShouldReturnAllTrainingPaths()
        {
            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Path1", result[0].Name);
            Assert.Equal("Path2", result[1].Name);
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnTrainingPath()
        {
            // Arrange
            var trainingPath = _trainingPaths[0];

            // Act
            var result = _service.GetById(trainingPath.TrainingPathId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(trainingPath.Name, result.Name);
            Assert.Equal(trainingPath.Description, result.Description);
        }

        [Fact]
        public void GetById_WithInvalidId_ShouldReturnNull()
        {
            // Act
            var result = _service.GetById(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Create_ShouldCreateNewTrainingPath()
        {
            // Arrange
            var newTrainingPath = new TrainingPath
            {
                TrainingPathId = Guid.NewGuid(),
                Name = "New Path",
                Description = "New Description",
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            // Act
            var result = _service.Create(newTrainingPath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newTrainingPath.Name, result.Name);
            Assert.Equal(newTrainingPath.Description, result.Description);
            Assert.Equal(3, _service.GetAll().Count);
        }

        [Fact]
        public void Update_WithValidId_ShouldUpdateTrainingPath()
        {
            // Arrange
            var trainingPath = _trainingPaths[0];
            var updatedTrainingPath = new TrainingPath
            {
                TrainingPathId = trainingPath.TrainingPathId,
                Name = "Updated Path",
                Description = "Updated Description",
                UpdateDate = DateTime.UtcNow
            };

            // Act
            var result = _service.Update(trainingPath.TrainingPathId, updatedTrainingPath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedTrainingPath.Name, result.Name);
            Assert.Equal(updatedTrainingPath.Description, result.Description);
            Assert.NotEqual(default(DateTime), result.UpdateDate);
        }

        [Fact]
        public void Delete_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            var trainingPath = _trainingPaths[0];

            // Act
            var result = _service.Delete(trainingPath.TrainingPathId);

            // Assert
            Assert.True(result);
            Assert.Equal(1, _service.GetAll().Count);
        }

        [Fact]
        public void Delete_WithInvalidId_ShouldReturnFalse()
        {
            // Act
            var result = _service.Delete(Guid.NewGuid());

            // Assert
            Assert.False(result);
            Assert.Equal(2, _service.GetAll().Count);
        }
    }
} 