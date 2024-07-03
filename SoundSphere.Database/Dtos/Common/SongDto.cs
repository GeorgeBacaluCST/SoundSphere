using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Common
{
    public class SongDto : BaseEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public Genre Genre { get; set; }

        public DateOnly ReleaseDate { get; set; }

        public int DurationSeconds { get; set; } = 0;

        public Guid AlbumId { get; set; }

        public IList<Guid> ArtistsIds { get; set; } = null!;

        public IList<Guid> SimilarSongsIds { get; set; } = null!;
    }
}