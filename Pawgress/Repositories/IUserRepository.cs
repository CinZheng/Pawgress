using Pawgress.Models;
using System;
using System.Collections.Generic;

namespace Pawgress.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User? GetById(Guid id);
        User Create(User user);
        User? Update(Guid id, User user);
        bool Delete(Guid id);
    }
} 