using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Common
{
    public class AlbumDto : BaseEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public DateOnly ReleaseDate { get; set; }

        public IList<Guid> SimilarAlbumsIds { get; set; } = null!;
    }
}