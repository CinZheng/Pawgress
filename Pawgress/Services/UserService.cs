using Pawgress.Data;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User? GetUserById(Guid id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }

        public User CreateUser(User newUser)
        {
            newUser.UserId = Guid.NewGuid();
            newUser.CreationDate = DateTime.Now;
            newUser.UpdateDate = DateTime.Now;
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }

        public User? UpdateUser(Guid id, User updatedUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return null;

            user.Username = updatedUser.Username ?? user.Username;
            user.Email = updatedUser.Email ?? user.Email;
            user.ProgressData = updatedUser.ProgressData ?? user.ProgressData;
            user.UpdateDate = DateTime.Now;

            _context.SaveChanges();
            return user;
        }

        public bool DeleteUser(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}
