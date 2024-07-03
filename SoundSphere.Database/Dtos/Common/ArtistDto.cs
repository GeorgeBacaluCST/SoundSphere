using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Common
{
    public class ArtistDto : BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string? Bio { get; set; }

        public IList<Guid> SimilarArtistsIds { get; set; } = null!;
    }
}