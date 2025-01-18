using Xunit;
using Moq;
using Pawgress.Models;
using Pawgress.Services;
using Pawgress.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pawgress.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _service = new UserService(_mockRepository.Object);
        }

        [Fact]
        public void GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = Guid.NewGuid(), Username = "User1" },
                new User { UserId = Guid.NewGuid(), Username = "User2" }
            };

            _mockRepository.Setup(repo => repo.GetAll())
                .Returns(users);

            // Act
            var result = _service.GetAllUsers();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("User1", result.First().Username);
            Assert.Equal("User2", result.Last().Username);
        }

        [Fact]
        public void GetUserById_WithValidId_ShouldReturnUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, Username = "TestUser" };

            _mockRepository.Setup(repo => repo.GetById(userId))
                .Returns(user);

            // Act
            var result = _service.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal("TestUser", result.Username);
        }

        [Fact]
        public void GetUserById_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetById(userId))
                .Returns((User)null);

            // Act
            var result = _service.GetUserById(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateUser_ShouldCreateNewUser()
        {
            // Arrange
            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                Username = "NewUser",
                Email = "test@example.com"
            };

            _mockRepository.Setup(repo => repo.Create(It.IsAny<User>()))
                .Returns(newUser);

            // Act
            var result = _service.CreateUser(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newUser.Username, result.Username);
            Assert.Equal(newUser.Email, result.Email);
            Assert.NotEqual(default, result.CreationDate);
            Assert.NotEqual(default, result.UpdateDate);
            _mockRepository.Verify(repo => repo.Create(It.Is<User>(u => 
                u.Username == newUser.Username && 
                u.Email == newUser.Email)), Times.Once);
        }



        [Fact]
        public void CreateUser_WithNullUser_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.CreateUser(null));
            _mockRepository.Verify(repo => repo.Create(It.IsAny<User>()), Times.Never);
        }


        [Fact]
        public void DeleteUser_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.Delete(userId))
                .Returns(true);

            // Act
            var result = _service.DeleteUser(userId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.Delete(userId), Times.Once);
        }

        [Fact]
        public void DeleteUser_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.Delete(userId))
                .Returns(false);

            // Act
            var result = _service.DeleteUser(userId);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.Delete(userId), Times.Once);
        }
    }
} 