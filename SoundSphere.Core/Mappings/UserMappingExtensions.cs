using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Mappings
{
    public static class UserMappingExtensions
    {
        public static IList<UserDto> ToDtos(this IList<User> users) => users.Select(user => user.ToDto()).ToList();

        public static IList<User> ToEntities(this IList<UserDto> userDtos, IRoleRepository roleRepository, IAuthorityRepository authorityRepository) => userDtos.Select(userDto => userDto.ToEntity(roleRepository, authorityRepository)).ToList();

        public static UserDto ToDto(this User user) => new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Mobile = user.Mobile,
            Address = user.Address,
            Birthday = user.Birthday,
            Avatar = user.Avatar,
            RoleId = user.Role.Id,
            AuthoritiesIds = user.Authorities.Select(authority => authority.Id).ToList(),
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            DeletedAt = user.DeletedAt
        };

        public static User ToEntity(this UserDto userDto, IRoleRepository roleRepository, IAuthorityRepository authorityRepository) => new User
        {
            Id = userDto.Id,
            Name = userDto.Name,
            Email = userDto.Email,
            Mobile = userDto.Mobile,
            Address = userDto.Address,
            Birthday = userDto.Birthday,
            Avatar = userDto.Avatar,
            Role = roleRepository.GetById(userDto.RoleId),
            Authorities = userDto.AuthoritiesIds.Select(authorityRepository.GetById).ToList(),
            CreatedAt = userDto.CreatedAt,
            UpdatedAt = userDto.UpdatedAt,
            DeletedAt = userDto.DeletedAt
        };
    }
}