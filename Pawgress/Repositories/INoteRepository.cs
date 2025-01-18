using Pawgress.Models;
using System;
using System.Collections.Generic;

namespace Pawgress.Repositories
{
    public interface INoteRepository
    {
        List<Note> GetAll();
        Note? GetById(Guid id);
        Note Create(Note note);
        Note? Update(Guid id, Note note);
        bool Delete(Guid id);
    }
} 