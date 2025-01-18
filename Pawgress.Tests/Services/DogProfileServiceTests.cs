using Moq;
using Xunit;
using Pawgress.Services;
using Pawgress.Models;
using Pawgress.Repositories;

namespace Pawgress.Tests.Services
{
    public class DogProfileServiceTests
    {
        private readonly Mock<IDogProfileRepository> _mockRepository;
        private readonly DogProfileService _service;

        public DogProfileServiceTests()
        {
            _mockRepository = new Mock<IDogProfileRepository>();
            _service = new DogProfileService(_mockRepository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllDogProfiles()
        {
            // Arrange
            var expectedDogProfiles = new List<DogProfile>
            {
                new DogProfile { DogProfileId = Guid.NewGuid(), Name = "Dog1" },
                new DogProfile { DogProfileId = Guid.NewGuid(), Name = "Dog2" }
            };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(expectedDogProfiles);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(expectedDogProfiles, result);
            _mockRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnDogProfile()
        {
            // Arrange
            var dogId = Guid.NewGuid();
            var expectedDogProfile = new DogProfile { DogProfileId = dogId, Name = "TestDog" };
            _mockRepository.Setup(repo => repo.GetById(dogId)).Returns(expectedDogProfile);

            // Act
            var result = _service.GetById(dogId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDogProfile, result);
            _mockRepository.Verify(repo => repo.GetById(dogId), Times.Once);
        }

        [Fact]
        public void GetById_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var dogId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetById(dogId)).Returns((DogProfile)null);

            // Act
            var result = _service.GetById(dogId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetById(dogId), Times.Once);
        }

        [Fact]
        public void Create_ShouldCreateNewDogProfile()
        {
            // Arrange
            var newDog = new DogProfile { Name = "NewDog" };
            _mockRepository.Setup(repo => repo.Create(It.IsAny<DogProfile>()))
                .Returns((DogProfile dog) => dog);

            // Act
            var result = _service.Create(newDog);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.DogProfileId);
            Assert.Equal(newDog.Name, result.Name);
            Assert.NotEqual(default(DateTime), result.CreationDate);
            Assert.NotEqual(default(DateTime), result.UpdateDate);
            _mockRepository.Verify(repo => repo.Create(It.IsAny<DogProfile>()), Times.Once);
        }



        [Fact]
        public void Delete_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            var dogId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.Delete(dogId)).Returns(true);

            // Act
            var result = _service.Delete(dogId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.Delete(dogId), Times.Once);
        }

        [Fact]
        public void Delete_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            var dogId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.Delete(dogId)).Returns(false);

            // Act
            var result = _service.Delete(dogId);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.Delete(dogId), Times.Once);
        }


        [Fact]
        public void Create_WithNullDogProfile_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(null));
        }

        [Fact]
        public void Update_WithNullDogProfile_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(Guid.NewGuid(), null));
        }
    }
} 