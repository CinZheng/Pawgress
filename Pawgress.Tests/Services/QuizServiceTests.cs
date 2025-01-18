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
    public class QuizServiceTests
    {
        private readonly Mock<IQuizRepository> _mockRepository;
        private readonly QuizService _service;

        public QuizServiceTests()
        {
            _mockRepository = new Mock<IQuizRepository>();
            _service = new QuizService(_mockRepository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllQuizzes()
        {
            // Arrange
            var expectedQuizzes = new List<Quiz>
            {
                new Quiz { Id = Guid.NewGuid(), Name = "Quiz 1" },
                new Quiz { Id = Guid.NewGuid(), Name = "Quiz 2" }
            };

            _mockRepository.Setup(repo => repo.GetAll())
                .Returns(expectedQuizzes);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(expectedQuizzes.Count, result.Count());
            Assert.Equal(expectedQuizzes[0].Name, result.First().Name);
            Assert.Equal(expectedQuizzes[1].Name, result.Last().Name);
            _mockRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnQuiz()
        {
            // Arrange
            var quizId = Guid.NewGuid();
            var expectedQuiz = new Quiz { Id = quizId, Name = "Test Quiz" };

            _mockRepository.Setup(repo => repo.GetById(quizId))
                .Returns(expectedQuiz);

            // Act
            var result = _service.GetById(quizId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedQuiz.Id, result.Id);
            Assert.Equal(expectedQuiz.Name, result.Name);
            _mockRepository.Verify(repo => repo.GetById(quizId), Times.Once);
        }

        [Fact]
        public void GetById_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetById(invalidId))
                .Returns((Quiz)null);

            // Act
            var result = _service.GetById(invalidId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetById(invalidId), Times.Once);
        }

        [Fact]
        public void Create_ShouldCreateNewQuiz()
        {
            // Arrange
            var quiz = new Quiz { Name = "New Quiz" };
            _mockRepository.Setup(repo => repo.Create(It.IsAny<Quiz>()))
                .Returns((Quiz q) => q);

            // Act
            var result = _service.Create(quiz);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(quiz.Name, result.Name);
            Assert.NotEqual(default, result.CreationDate);
            Assert.NotEqual(default, result.UpdateDate);
            _mockRepository.Verify(repo => repo.Create(It.IsAny<Quiz>()), Times.Once);
        }



        [Fact]
        public void Delete_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            var quizId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.Delete(quizId))
                .Returns(true);

            // Act
            var result = _service.Delete(quizId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.Delete(quizId), Times.Once);
        }

        [Fact]
        public void Delete_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.Delete(invalidId))
                .Returns(false);

            // Act
            var result = _service.Delete(invalidId);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.Delete(invalidId), Times.Once);
        }

        [Fact]
        public void AddQuestion_WithValidQuizId_ShouldAddQuestionToQuiz()
        {
            // Arrange
            var quizId = Guid.NewGuid();
            var quiz = new Quiz 
            { 
                Id = quizId, 
                Name = "Test Quiz",
                QuizQuestions = new List<QuizQuestion>()
            };
            var question = new QuizQuestion 
            { 
                QuizQuestionId = Guid.NewGuid(),
                QuestionText = "Test Question",
                CorrectAnswer = "Test Answer"
            };

            _mockRepository.Setup(repo => repo.GetById(quizId))
                .Returns(quiz);
            _mockRepository.Setup(repo => repo.Update(quizId, It.IsAny<Quiz>()))
                .Returns((Guid id, Quiz q) => q);

            // Act
            var result = _service.AddQuestion(quizId, question);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.QuizQuestions);
            Assert.Equal(question.QuestionText, result.QuizQuestions[0].QuestionText);
            Assert.Equal(question.CorrectAnswer, result.QuizQuestions[0].CorrectAnswer);
        }

        [Fact]
        public void AddQuestion_WithInvalidQuizId_ShouldThrowException()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var question = new QuizQuestion 
            { 
                QuizQuestionId = Guid.NewGuid(),
                QuestionText = "Test Question",
                CorrectAnswer = "Test Answer"
            };

            _mockRepository.Setup(repo => repo.GetById(invalidId))
                .Returns((Quiz)null);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _service.AddQuestion(invalidId, question));
            Assert.Equal("Quiz niet gevonden.", exception.Message);
            _mockRepository.Verify(repo => repo.GetById(invalidId), Times.Once);
            _mockRepository.Verify(repo => repo.Update(invalidId, It.IsAny<Quiz>()), Times.Never);
        }

        [Fact]
        public void Create_WithNullQuiz_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(null));
            _mockRepository.Verify(repo => repo.Create(It.IsAny<Quiz>()), Times.Never);
        }

        [Fact]
        public void Update_WithNullQuiz_ShouldThrowArgumentNullException()
        {
            // Arrange
            var quizId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(quizId, null));
            _mockRepository.Verify(repo => repo.Update(quizId, It.IsAny<Quiz>()), Times.Never);
        }

        [Fact]
        public void AddQuestion_WithNullQuestion_ShouldThrowArgumentNullException()
        {
            // Arrange
            var quizId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.AddQuestion(quizId, null));
            _mockRepository.Verify(repo => repo.GetById(quizId), Times.Never);
            _mockRepository.Verify(repo => repo.Update(quizId, It.IsAny<Quiz>()), Times.Never);
        }
    }
} 