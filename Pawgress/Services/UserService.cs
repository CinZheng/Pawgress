using Pawgress.Repositories;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public User? GetUserById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        public User CreateUser(User newUser)
        {
            newUser.UserId = Guid.NewGuid();
            newUser.CreationDate = DateTime.Now;
            newUser.UpdateDate = DateTime.Now;
            return _userRepository.Create(newUser);
        }

        public User? UpdateUser(Guid id, User updatedUser)
        {
            return _userRepository.Update(id, updatedUser);
        }

        public bool DeleteUser(Guid id)
        {
            return _userRepository.Delete(id);
        }
    }
}
