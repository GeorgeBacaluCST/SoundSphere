using System.Text.Json.Serialization;

namespace SoundSphere.Database.Entities
{
    public class Song : BaseEntity
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = null!;
        
        public string ImageUrl { get; set; } = null!;
        
        public Genre Genre { get; set; }
        
        public DateOnly ReleaseDate { get; set; }
        
        public int DurationSeconds { get; set; }
        
        public Album Album { get; set; } = null!;
        
        public List<Artist> Artists { get; set; } = new()!;

        [JsonIgnore] public List<Playlist>? Playlists { get; set; } = new()!;

        public List<SongLink> SimilarSongs { get; set; } = new()!;
    }

    public enum Genre { InvalidGenre, Pop, Rock, Rnb, HipHop, Dance, Techno, Latino, Hindi, Reggae, Jazz, Classical, Country, Electronic }
}