using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) => _userRepository = userRepository;

        public IList<User> GetAll() => _userRepository.GetAll();

        public User GetById(Guid id) => _userRepository.GetById(id);

        public User Add(User user)
        {
            _userRepository.LinkUserToRole(user);
            _userRepository.LinkUserToAuthorities(user);
            _userRepository.AddUserArtist(user);
            _userRepository.AddUserSong(user);
            return _userRepository.Add(user);
        }

        public User UpdateById(User user, Guid id) => _userRepository.UpdateById(user, id);

        public User DeleteById(Guid id) => _userRepository.DeleteById(id);
    }
}