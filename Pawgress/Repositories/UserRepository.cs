using Pawgress.Data;
using Pawgress.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pawgress.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User? GetById(Guid id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User? Update(Guid id, User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (existingUser == null) return null;

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.ProgressData = user.ProgressData;
            existingUser.UpdateDate = DateTime.Now;

            _context.SaveChanges();
            return existingUser;
        }

        public bool Delete(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
} 