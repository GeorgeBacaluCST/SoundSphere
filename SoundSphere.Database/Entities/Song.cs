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
        
        public IList<Artist> Artists { get; set; } = null!;
        
        [JsonIgnore] public IList<Playlist>? Playlists { get; set; }
        
        public IList<SongLink> SimilarSongs { get; set; } = null!;
    }

    public enum Genre { InvalidGenre, Pop, Rock, Rnb, HipHop, Dance, Techno, Latino, Hindi, Reggae, Jazz, Classical, Country, Electronic }
}