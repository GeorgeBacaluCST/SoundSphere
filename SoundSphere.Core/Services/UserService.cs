using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthorityRepository _authorityRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IAuthorityRepository authorityRepository) =>
            (_userRepository, _roleRepository, _authorityRepository) = (userRepository, roleRepository, authorityRepository);

        public IList<UserDto> GetAll() => _userRepository.GetAll().ToDtos();

        public UserDto GetById(Guid id) => _userRepository.GetById(id).ToDto();

        public UserDto Add(UserDto userDto)
        {
            User userToAdd = userDto.ToEntity(_roleRepository, _authorityRepository);
            _userRepository.LinkUserToRole(userToAdd);
            _userRepository.LinkUserToAuthorities(userToAdd);
            _userRepository.AddUserArtist(userToAdd);
            _userRepository.AddUserSong(userToAdd);
            return _userRepository.Add(userToAdd).ToDto();
        }

        public UserDto UpdateById(UserDto userDto, Guid id) => _userRepository.UpdateById(userDto.ToEntity(_roleRepository, _authorityRepository), id).ToDto();

        public UserDto DeleteById(Guid id) => _userRepository.DeleteById(id).ToDto();
    }
}