using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Common
{
    public class UserDto : BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Mobile { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateOnly Birthday { get; set; }

        public string Avatar { get; set; } = null!;

        public Guid RoleId { get; set; }

        public IList<Guid> AuthoritiesIds { get; set; } = null!;
    }
}