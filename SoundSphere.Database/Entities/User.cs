using System.Text.Json.Serialization;

namespace SoundSphere.Database.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public string Email { get; set; } = null!;
        
        public string Password { get; set; } = null!;
        
        public string Mobile { get; set; } = null!;
        
        public string Address { get; set; } = null!;
        
        public DateOnly Birthday { get; set; }
        
        public string Avatar { get; set; } = null!;
        
        public Role Role { get; set; } = null!;
        
        public List<Authority> Authorities { get; set; } = new()!;

        [JsonIgnore] public List<UserSong>? UserSongs { get; set; } = new()!;

        [JsonIgnore] public List<UserArtist>? UserArtists { get; set; } = new()!;
    }
}