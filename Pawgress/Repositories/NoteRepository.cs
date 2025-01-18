using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using Pawgress.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pawgress.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext _context;

        public NoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Note> GetAll()
        {
            return _context.Notes
                .Include(n => n.User)
                .ToList();
        }

        public Note? GetById(Guid id)
        {
            return _context.Notes
                .Include(n => n.User)
                .FirstOrDefault(n => n.NoteId == id);
        }

        public Note Create(Note note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
            return note;
        }

        public Note? Update(Guid id, Note note)
        {
            var existingNote = _context.Notes.FirstOrDefault(n => n.NoteId == id);
            if (existingNote == null) return null;

            existingNote.Description = note.Description;
            existingNote.Tag = note.Tag;
            existingNote.UpdateDate = DateTime.Now;

            _context.SaveChanges();
            return existingNote;
        }

        public bool Delete(Guid id)
        {
            var note = _context.Notes.FirstOrDefault(n => n.NoteId == id);
            if (note == null) return false;

            _context.Notes.Remove(note);
            _context.SaveChanges();
            return true;
        }
    }
} 