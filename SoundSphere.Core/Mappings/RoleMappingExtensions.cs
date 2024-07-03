using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Mappings
{
    public static class RoleMappingExtensions
    {
        public static IList<RoleDto> ToDtos(this IList<Role> roles) => roles.Select(role => role.ToDto()).ToList();

        public static IList<Role> ToEntities(this IList<RoleDto> roleDtos) => roleDtos.Select(roleDto => roleDto.ToEntity()).ToList();

        public static RoleDto ToDto(this Role role) => new RoleDto { Id = role.Id, Type = role.Type, CreatedAt = role.CreatedAt };

        public static Role ToEntity(this RoleDto roleDto) => new Role { Id = roleDto.Id, Type = roleDto.Type, CreatedAt = roleDto.CreatedAt };
    }
}