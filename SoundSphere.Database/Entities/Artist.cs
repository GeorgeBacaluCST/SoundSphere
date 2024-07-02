using System.Text.Json.Serialization;

namespace SoundSphere.Database.Entities
{
    public class Artist : BaseEntity
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public string ImageUrl { get; set; } = null!;
        
        public string? Bio { get; set; }
        
        [JsonIgnore] public IList<Song>? Songs { get; set; }
        
        public IList<ArtistLink> SimilarArtists { get; set; } = null!;
    }
}