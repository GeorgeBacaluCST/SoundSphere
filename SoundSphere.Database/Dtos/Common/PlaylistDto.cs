using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Common
{
    public class PlaylistDto : BaseEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public Guid UserId { get; set; }

        public IList<Guid> SongsIds { get; set; } = null!;
    }
}