using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Common
{
    public class RoleDto
    {
        public Guid Id { get; set; }

        public RoleType Type { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}