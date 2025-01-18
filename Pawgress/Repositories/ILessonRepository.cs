using Pawgress.Models;
using System;
using System.Collections.Generic;

namespace Pawgress.Repositories
{
    public interface ILessonRepository
    {
        List<Lesson> GetAll();
        Lesson? GetById(Guid id);
        Lesson Create(Lesson lesson);
        Lesson? Update(Guid id, Lesson lesson);
        bool Delete(Guid id);
    }
} 