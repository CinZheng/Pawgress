using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using Pawgress.Models;
using Pawgress.Services;
using Pawgress.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pawgress.Tests.Services
{
    public class LessonServiceTests
    {
        private readonly Mock<ILessonRepository> _mockRepository;
        private readonly LessonService _service;

        public LessonServiceTests()
        {
            _mockRepository = new Mock<ILessonRepository>();
            _service = new LessonService(_mockRepository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllLessons()
        {
            // Arrange
            var lessons = new List<Lesson>
            {
                new Lesson { Id = Guid.NewGuid(), Name = "Lesson1" },
                new Lesson { Id = Guid.NewGuid(), Name = "Lesson2" }
            };

            _mockRepository.Setup(repo => repo.GetAll())
                .Returns(lessons);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Lesson1", result.First().Name);
            Assert.Equal("Lesson2", result.Last().Name);
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnLesson()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            var lesson = new Lesson { Id = lessonId, Name = "TestLesson" };

            _mockRepository.Setup(repo => repo.GetById(lessonId))
                .Returns(lesson);

            // Act
            var result = _service.GetById(lessonId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(lessonId, result.Id);
            Assert.Equal("TestLesson", result.Name);
        }

        [Fact]
        public void GetById_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetById(lessonId))
                .Returns((Lesson)null);

            // Act
            var result = _service.GetById(lessonId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Create_ShouldCreateNewLesson()
        {
            // Arrange
            var newLesson = new Lesson
            {
                Id = Guid.NewGuid(),
                Name = "New Lesson",
                Text = "Lesson Content",
                CreationDate = DateTime.Now
            };

            _mockRepository.Setup(repo => repo.Create(It.IsAny<Lesson>()))
                .Returns(newLesson);

            // Act
            var result = _service.Create(newLesson);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newLesson.Name, result.Name);
            Assert.Equal(newLesson.Text, result.Text);
            _mockRepository.Verify(repo => repo.Create(It.Is<Lesson>(l => 
                l.Name == newLesson.Name && 
                l.Text == newLesson.Text)), Times.Once);
        }




        [Fact]
        public void Create_WithNullLesson_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(null));
            _mockRepository.Verify(repo => repo.Create(It.IsAny<Lesson>()), Times.Never);
        }

        [Fact]
        public void Update_WithNullLesson_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(Guid.NewGuid(), null));
            _mockRepository.Verify(repo => repo.Update(It.IsAny<Guid>(), It.IsAny<Lesson>()), Times.Never);
        }

        [Fact]
        public void Delete_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.Delete(lessonId))
                .Returns(true);

            // Act
            var result = _service.Delete(lessonId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.Delete(lessonId), Times.Once);
        }

        [Fact]
        public void Delete_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.Delete(lessonId))
                .Returns(false);

            // Act
            var result = _service.Delete(lessonId);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.Delete(lessonId), Times.Once);
        }
    }
} 