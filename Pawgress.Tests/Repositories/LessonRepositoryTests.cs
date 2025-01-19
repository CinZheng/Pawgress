using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using Pawgress.Models;
using Pawgress.Repositories;
using Xunit;

namespace Pawgress.Tests.Repositories
{
    public class LessonRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly LessonRepository _repository;
        private readonly Lesson _testLesson;

        public LessonRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestLessonDb_" + Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new LessonRepository(_context);

            // Create a test lesson
            _testLesson = new Lesson
            {
                Id = Guid.NewGuid(),
                Name = "Test Lesson",
                Text = "Test Text",
                Video = "test-video.mp4",
                Image = "test-image.jpg",
                MediaUrl = "https://example.com/media",
                Tag = "Test Tag",
                MarkdownContent = "# Test Content",
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            _context.Lessons.Add(_testLesson);
            _context.SaveChanges();
        }

        [Fact]
        public void GetAll_ShouldReturnAllLessons()
        {
            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.Single(result);
            var lesson = result.First();
            Assert.Equal(_testLesson.Name, lesson.Name);
            Assert.Equal(_testLesson.Text, lesson.Text);
            Assert.Equal(_testLesson.Video, lesson.Video);
            Assert.Equal(_testLesson.Image, lesson.Image);
            Assert.Equal(_testLesson.MediaUrl, lesson.MediaUrl);
            Assert.Equal(_testLesson.Tag, lesson.Tag);
            Assert.Equal(_testLesson.MarkdownContent, lesson.MarkdownContent);
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnLesson()
        {
            // Act
            var result = _repository.GetById(_testLesson.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_testLesson.Name, result.Name);
            Assert.Equal(_testLesson.Text, result.Text);
            Assert.Equal(_testLesson.Video, result.Video);
            Assert.Equal(_testLesson.Image, result.Image);
            Assert.Equal(_testLesson.MediaUrl, result.MediaUrl);
            Assert.Equal(_testLesson.Tag, result.Tag);
            Assert.Equal(_testLesson.MarkdownContent, result.MarkdownContent);
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
        public void Create_ShouldAddNewLesson()
        {
            // Arrange
            var newLesson = new Lesson
            {
                Id = Guid.NewGuid(),
                Name = "New Lesson",
                Text = "New Text",
                Video = "new-video.mp4",
                Image = "new-image.jpg",
                MediaUrl = "https://example.com/new-media",
                Tag = "New Tag",
                MarkdownContent = "# New Content",
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            // Act
            var result = _repository.Create(newLesson);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newLesson.Name, result.Name);
            Assert.Equal(newLesson.Text, result.Text);
            Assert.Equal(newLesson.Video, result.Video);
            Assert.Equal(newLesson.Image, result.Image);
            Assert.Equal(newLesson.MediaUrl, result.MediaUrl);
            Assert.Equal(newLesson.Tag, result.Tag);
            Assert.Equal(newLesson.MarkdownContent, result.MarkdownContent);
            Assert.Equal(2, _context.Lessons.Count());
        }

        [Fact]
        public void Update_WithValidId_ShouldUpdateLesson()
        {
            // Arrange
            var updatedLesson = new Lesson
            {
                Id = _testLesson.Id,
                Name = "Updated Lesson",
                Text = "Updated Text",
                Video = "updated-video.mp4",
                Image = "updated-image.jpg",
                MediaUrl = "https://example.com/updated-media",
                Tag = "Updated Tag",
                MarkdownContent = "# Updated Content"
            };

            // Act
            var result = _repository.Update(_testLesson.Id, updatedLesson);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Lesson", result.Name);
            Assert.Equal("Updated Text", result.Text);
            Assert.Equal("updated-video.mp4", result.Video);
            Assert.Equal("updated-image.jpg", result.Image);
            Assert.Equal("https://example.com/updated-media", result.MediaUrl);
            Assert.Equal("Updated Tag", result.Tag);
            Assert.Equal("# Updated Content", result.MarkdownContent);
            
            // Verify in context
            var lessonInDb = _context.Lessons.Find(_testLesson.Id);
            Assert.NotNull(lessonInDb);
            Assert.Equal("Updated Lesson", lessonInDb.Name);
            Assert.Equal("Updated Text", lessonInDb.Text);
            Assert.Equal("updated-video.mp4", lessonInDb.Video);
            Assert.Equal("updated-image.jpg", lessonInDb.Image);
            Assert.Equal("https://example.com/updated-media", lessonInDb.MediaUrl);
            Assert.Equal("Updated Tag", lessonInDb.Tag);
            Assert.Equal("# Updated Content", lessonInDb.MarkdownContent);
        }

        [Fact]
        public void Update_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var updatedLesson = new Lesson
            {
                Id = invalidId,
                Name = "Updated Lesson",
                Text = "Updated Text"
            };

            // Act
            var result = _repository.Update(invalidId, updatedLesson);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Delete_WithValidId_ShouldReturnTrue()
        {
            // Act
            var result = _repository.Delete(_testLesson.Id);

            // Assert
            Assert.True(result);
            Assert.Empty(_context.Lessons);
        }

        [Fact]
        public void Delete_WithInvalidId_ShouldReturnFalse()
        {
            // Act
            var result = _repository.Delete(Guid.NewGuid());

            // Assert
            Assert.False(result);
            Assert.Single(_context.Lessons);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
} 