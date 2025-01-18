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
    public class NoteServiceTests
    {
        private readonly Mock<INoteRepository> _mockNoteRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly NoteService _service;

        public NoteServiceTests()
        {
            _mockNoteRepository = new Mock<INoteRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _service = new NoteService(_mockNoteRepository.Object, _mockUserRepository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllNotes()
        {
            // Arrange
            var user = new User { UserId = Guid.NewGuid(), Username = "TestUser" };
            var notes = new List<Note>
            {
                new Note { NoteId = Guid.NewGuid(), Description = "Note1", UserId = user.UserId },
                new Note { NoteId = Guid.NewGuid(), Description = "Note2", UserId = user.UserId }
            };

            _mockNoteRepository.Setup(repo => repo.GetAll())
                .Returns(notes);
            
            _mockUserRepository.Setup(repo => repo.GetById(user.UserId))
                .Returns(user);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Note1", result.First().Description);
            Assert.Equal("Note2", result.Last().Description);
            Assert.Equal("TestUser", result.First().User.Username);
            _mockUserRepository.Verify(repo => repo.GetById(user.UserId), Times.Exactly(2));
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnNote()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var user = new User { UserId = Guid.NewGuid(), Username = "TestUser" };
            var note = new Note { NoteId = noteId, Description = "TestNote", User = user };

            _mockNoteRepository.Setup(repo => repo.GetById(noteId))
                .Returns(note);

            // Act
            var result = _service.GetById(noteId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(noteId, result.NoteId);
            Assert.Equal("TestNote", result.Description);
            Assert.NotNull(result.User);
            Assert.Equal("TestUser", result.User.Username);
        }

        [Fact]
        public void GetById_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            _mockNoteRepository.Setup(repo => repo.GetById(noteId))
                .Returns((Note)null);

            // Act
            var result = _service.GetById(noteId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Create_ShouldCreateNewNote()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newNote = new Note
            {
                NoteId = Guid.NewGuid(),
                Description = "New Note",
                Tag = "Test",
                UserId = userId,
                CreationDate = DateTime.Now
            };

            _mockNoteRepository.Setup(repo => repo.Create(It.IsAny<Note>()))
                .Returns(newNote);

            // Act
            var result = _service.Create(newNote);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newNote.Description, result.Description);
            Assert.Equal(newNote.Tag, result.Tag);
            _mockNoteRepository.Verify(repo => repo.Create(It.Is<Note>(n => 
                n.Description == newNote.Description && 
                n.Tag == newNote.Tag)), Times.Once);
        }



        [Fact]
        public void Create_WithNullNote_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(null));
            _mockNoteRepository.Verify(repo => repo.Create(It.IsAny<Note>()), Times.Never);
        }

        [Fact]
        public void Update_WithNullNote_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(Guid.NewGuid(), null));
            _mockNoteRepository.Verify(repo => repo.Update(It.IsAny<Guid>(), It.IsAny<Note>()), Times.Never);
        }

        [Fact]
        public void Delete_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            _mockNoteRepository.Setup(repo => repo.Delete(noteId))
                .Returns(true);

            // Act
            var result = _service.Delete(noteId);

            // Assert
            Assert.True(result);
            _mockNoteRepository.Verify(repo => repo.Delete(noteId), Times.Once);
        }

        [Fact]
        public void Delete_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            _mockNoteRepository.Setup(repo => repo.Delete(noteId))
                .Returns(false);

            // Act
            var result = _service.Delete(noteId);

            // Assert
            Assert.False(result);
            _mockNoteRepository.Verify(repo => repo.Delete(noteId), Times.Once);
        }
    }
} 