using Pawgress.Models;
using System;
using System.Collections.Generic;

namespace Pawgress.Repositories
{
    public interface IDogProfileRepository
    {
        List<DogProfile> GetAll();
        DogProfile? GetById(Guid id);
        DogProfile Create(DogProfile dogProfile);
        DogProfile? Update(Guid id, DogProfile dogProfile);
        bool Delete(Guid id);
    }
} 