using Pawgress.Repositories;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll() ?? new List<User>();
        }

        public User? GetUserById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        public User CreateUser(User newUser)
        {
            if (newUser == null) throw new ArgumentNullException(nameof(newUser));

            newUser.UserId = Guid.NewGuid();
            newUser.CreationDate = DateTime.UtcNow;
            newUser.UpdateDate = DateTime.UtcNow;
            return _userRepository.Create(newUser);
        }

        public User? UpdateUser(Guid id, User updatedUser)
        {
            if (updatedUser == null) throw new ArgumentNullException(nameof(updatedUser));

            var existingUser = GetUserById(id);
            if (existingUser == null) return null;

            updatedUser.CreationDate = existingUser.CreationDate;
            updatedUser.UpdateDate = DateTime.UtcNow;
            return _userRepository.Update(id, updatedUser);
        }

        public bool DeleteUser(Guid id)
        {
            return _userRepository.Delete(id);
        }
    }
}
