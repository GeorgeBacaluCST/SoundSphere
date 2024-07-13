using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Test.Mocks
{
    public class RoleMock
    {
        private RoleMock() { }

        public static List<Role> GetRoles() => [GetRole1(), GetRole2(), GetRole3()];

        public static List<RoleDto> GetRoleDtos() => GetRoles().Select(ToDto).ToList();

        public static Role GetRole1() => new() { Id = Guid.Parse("deaf35ba-fe71-4c21-8a3c-d8e5b32a06fe"), Type = RoleType.Admin, CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0) };

        public static Role GetRole2() => new() { Id = Guid.Parse("2fc8f207-5af0-402f-84d0-e1c7fa7336a6"), Type = RoleType.Moderator, CreatedAt = new DateTime(2024, 7, 1, 0, 0, 1) };

        public static Role GetRole3() => new() { Id = Guid.Parse("61ee6dda-e18a-4eb9-a736-3f95ba5537f7"), Type = RoleType.Listener, CreatedAt = new DateTime(2024, 7, 1, 0, 0, 2) };

        public static RoleDto GetRoleDto1() => ToDto(GetRole1());

        public static RoleDto GetRoleDto2() => ToDto(GetRole2());

        public static RoleDto GetRoleDto3() => ToDto(GetRole3());

        public static RoleDto ToDto(Role role) => new() { Id = role.Id, Type = role.Type, CreatedAt = role.CreatedAt };
    }
}