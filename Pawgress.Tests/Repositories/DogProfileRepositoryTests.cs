using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using Pawgress.Models;
using Pawgress.Repositories;
using Xunit;

namespace Pawgress.Tests.Repositories
{
    public class DogProfileRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly DogProfileRepository _repository;
        private readonly DogProfile _testDogProfile;

        public DogProfileRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDogProfileDb_" + Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new DogProfileRepository(_context);

            // Create a test dog profile
            _testDogProfile = new DogProfile
            {
                DogProfileId = Guid.NewGuid(),
                Name = "Test Dog",
                Breed = "Test Breed",
                Image = "test-image.jpg",
                DateOfBirth = DateTime.UtcNow.AddYears(-2),
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            _context.DogProfiles.Add(_testDogProfile);
            _context.SaveChanges();
        }

        [Fact]
        public void GetAll_ShouldReturnAllDogProfiles()
        {
            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.Single(result);
            var dogProfile = result.First();
            Assert.Equal(_testDogProfile.Name, dogProfile.Name);
            Assert.Equal(_testDogProfile.Breed, dogProfile.Breed);
            Assert.Equal(_testDogProfile.Image, dogProfile.Image);
            Assert.Equal(_testDogProfile.DateOfBirth, dogProfile.DateOfBirth);
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnDogProfile()
        {
            // Act
            var result = _repository.GetById(_testDogProfile.DogProfileId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_testDogProfile.Name, result.Name);
            Assert.Equal(_testDogProfile.Breed, result.Breed);
            Assert.Equal(_testDogProfile.Image, result.Image);
            Assert.Equal(_testDogProfile.DateOfBirth, result.DateOfBirth);
        }

        [Fact]
        public void GetById_WithInvalidId_ShouldReturnNull()
        {
            // Act
            var result = _repository.GetById(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Create_ShouldAddNewDogProfile()
        {
            // Arrange
            var newDogProfile = new DogProfile
            {
                DogProfileId = Guid.NewGuid(),
                Name = "New Dog",
                Breed = "New Breed",
                Image = "new-image.jpg",
                DateOfBirth = DateTime.UtcNow.AddYears(-1),
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            // Act
            var result = _repository.Create(newDogProfile);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newDogProfile.Name, result.Name);
            Assert.Equal(newDogProfile.Breed, result.Breed);
            Assert.Equal(newDogProfile.Image, result.Image);
            Assert.Equal(newDogProfile.DateOfBirth, result.DateOfBirth);
            Assert.Equal(2, _context.DogProfiles.Count());
        }

        [Fact]
        public void Update_WithValidId_ShouldUpdateDogProfile()
        {
            // Arrange
            var updatedDogProfile = new DogProfile
            {
                DogProfileId = _testDogProfile.DogProfileId,
                Name = "Updated Dog",
                Breed = "Updated Breed",
                Image = "updated-image.jpg",
                DateOfBirth = DateTime.UtcNow.AddYears(-3)
            };

            // Act
            var result = _repository.Update(_testDogProfile.DogProfileId, updatedDogProfile);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Dog", result.Name);
            Assert.Equal("Updated Breed", result.Breed);
            Assert.Equal("updated-image.jpg", result.Image);
            Assert.Equal(updatedDogProfile.DateOfBirth, result.DateOfBirth);
            
            // Verify in context
            var dogProfileInDb = _context.DogProfiles.Find(_testDogProfile.DogProfileId);
            Assert.NotNull(dogProfileInDb);
            Assert.Equal("Updated Dog", dogProfileInDb.Name);
            Assert.Equal("Updated Breed", dogProfileInDb.Breed);
            Assert.Equal("updated-image.jpg", dogProfileInDb.Image);
            Assert.Equal(updatedDogProfile.DateOfBirth, dogProfileInDb.DateOfBirth);
        }

        [Fact]
        public void Update_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var updatedDogProfile = new DogProfile
            {
                DogProfileId = invalidId,
                Name = "Updated Dog",
                Breed = "Updated Breed"
            };

            // Act
            var result = _repository.Update(invalidId, updatedDogProfile);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Delete_WithValidId_ShouldReturnTrue()
        {
            // Act
            var result = _repository.Delete(_testDogProfile.DogProfileId);

            // Assert
            Assert.True(result);
            Assert.Empty(_context.DogProfiles);
        }

        [Fact]
        public void Delete_WithInvalidId_ShouldReturnFalse()
        {
            // Act
            var result = _repository.Delete(Guid.NewGuid());

            // Assert
            Assert.False(result);
            Assert.Single(_context.DogProfiles);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
} 