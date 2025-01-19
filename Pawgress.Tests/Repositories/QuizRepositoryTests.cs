using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using Pawgress.Models;
using Pawgress.Repositories;
using Xunit;

namespace Pawgress.Tests.Repositories
{
    public class QuizRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly QuizRepository _repository;
        private readonly Quiz _testQuiz;

        public QuizRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestQuizDb_" + Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new QuizRepository(_context);

            // Create a test quiz
            _testQuiz = new Quiz
            {
                Id = Guid.NewGuid(),
                Name = "Test Quiz",
                Description = "Test Description",
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            _context.Quizzes.Add(_testQuiz);
            _context.SaveChanges();
        }

        [Fact]
        public void GetAll_ShouldReturnAllQuizzes()
        {
            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.Single(result);
            var quiz = result.First();
            Assert.Equal(_testQuiz.Name, quiz.Name);
            Assert.Equal(_testQuiz.Description, quiz.Description);
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnQuiz()
        {
            // Act
            var result = _repository.GetById(_testQuiz.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_testQuiz.Name, result.Name);
            Assert.Equal(_testQuiz.Description, result.Description);
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
        public void Create_ShouldAddNewQuiz()
        {
            // Arrange
            var newQuiz = new Quiz
            {
                Id = Guid.NewGuid(),
                Name = "New Quiz",
                Description = "New Description",
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            // Act
            var result = _repository.Create(newQuiz);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newQuiz.Name, result.Name);
            Assert.Equal(newQuiz.Description, result.Description);
            Assert.Equal(2, _context.Quizzes.Count());
        }

        [Fact]
        public void Update_WithValidId_ShouldUpdateQuiz()
        {
            // Arrange
            var updatedQuiz = new Quiz
            {
                Id = _testQuiz.Id,
                Name = "Updated Quiz",
                Description = "Updated Description"
            };

            // Act
            var result = _repository.Update(_testQuiz.Id, updatedQuiz);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Quiz", result.Name);
            Assert.Equal("Updated Description", result.Description);
            
            // Verify in context
            var quizInDb = _context.Quizzes.Find(_testQuiz.Id);
            Assert.NotNull(quizInDb);
            Assert.Equal("Updated Quiz", quizInDb.Name);
            Assert.Equal("Updated Description", quizInDb.Description);
        }

        [Fact]
        public void Update_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var updatedQuiz = new Quiz
            {
                Id = invalidId,
                Name = "Updated Quiz",
                Description = "Updated Description"
            };

            // Act
            var result = _repository.Update(invalidId, updatedQuiz);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Delete_WithValidId_ShouldReturnTrue()
        {
            // Act
            var result = _repository.Delete(_testQuiz.Id);

            // Assert
            Assert.True(result);
            Assert.Empty(_context.Quizzes);
        }

        [Fact]
        public void Delete_WithInvalidId_ShouldReturnFalse()
        {
            // Act
            var result = _repository.Delete(Guid.NewGuid());

            // Assert
            Assert.False(result);
            Assert.Single(_context.Quizzes);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
} 