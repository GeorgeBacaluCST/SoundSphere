using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Common
{
    public class AuthorityDto
    {
        public Guid Id { get; set; }

        public AuthorityType Type { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}