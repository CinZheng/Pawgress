using Pawgress.Models;
using System;
using System.Collections.Generic;

namespace Pawgress.Repositories
{
    public interface IQuizRepository
    {
        List<Quiz> GetAll();
        Quiz? GetById(Guid id);
        Quiz Create(Quiz quiz);
        Quiz? Update(Guid id, Quiz quiz);
        bool Delete(Guid id);
    }
} 