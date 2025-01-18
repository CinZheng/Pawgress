using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using Pawgress.Models;
using Pawgress.Repositories;
using Xunit;

namespace Pawgress.Tests.Repositories
{
    public class NoteRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly NoteRepository _repository;
        private readonly Note _testNote;
        private readonly User _testUser;

        public NoteRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestNoteDb_" + Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new NoteRepository(_context);

            // Create a test user first
            _testUser = new User
            {
                UserId = Guid.NewGuid(),
                Username = "TestUser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword",
                Role = "User"
            };
            _context.Users.Add(_testUser);

            // Create a test note with the user
            _testNote = new Note
            {
                NoteId = Guid.NewGuid(),
                Description = "Test Note",
                UserId = _testUser.UserId,
                User = _testUser,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            _context.Notes.Add(_testNote);
            _context.SaveChanges();
        }

        [Fact]
        public void GetAll_ShouldReturnAllNotesWithUsers()
        {
            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.Single(result);
            var note = result.First();
            Assert.Equal(_testNote.Description, note.Description);
            Assert.NotNull(note.User);
            Assert.Equal(_testUser.Username, note.User.Username);
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnNoteWithUser()
        {
            // Act
            var result = _repository.GetById(_testNote.NoteId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_testNote.Description, result.Description);
            Assert.NotNull(result.User);
            Assert.Equal(_testUser.Username, result.User.Username);
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
        public void Create_ShouldAddNewNote()
        {
            // Arrange
            var newNote = new Note
            {
                NoteId = Guid.NewGuid(),
                Description = "New Note",
                UserId = _testUser.UserId,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            // Act
            var result = _repository.Create(newNote);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newNote.Description, result.Description);
            Assert.Equal(2, _context.Notes.Count());
        }

        [Fact]
        public void Update_WithValidId_ShouldUpdateNote()
        {
            // Arrange
            var updatedNote = new Note
            {
                NoteId = _testNote.NoteId,
                Description = "Updated Note",
                Tag = "NewTag",
                UserId = _testUser.UserId
            };

            // Act
            var result = _repository.Update(_testNote.NoteId, updatedNote);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Note", result.Description);
            Assert.Equal("NewTag", result.Tag);
            
            // Verify in context
            var noteInDb = _context.Notes.Find(_testNote.NoteId);
            Assert.NotNull(noteInDb);
            Assert.Equal("Updated Note", noteInDb.Description);
            Assert.Equal("NewTag", noteInDb.Tag);
        }

        [Fact]
        public void Update_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var updatedNote = new Note
            {
                NoteId = invalidId,
                Description = "Updated Note"
            };

            // Act
            var result = _repository.Update(invalidId, updatedNote);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Delete_WithValidId_ShouldReturnTrue()
        {
            // Act
            var result = _repository.Delete(_testNote.NoteId);

            // Assert
            Assert.True(result);
            Assert.Empty(_context.Notes);
        }

        [Fact]
        public void Delete_WithInvalidId_ShouldReturnFalse()
        {
            // Act
            var result = _repository.Delete(Guid.NewGuid());

            // Assert
            Assert.False(result);
            Assert.Single(_context.Notes);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
} 