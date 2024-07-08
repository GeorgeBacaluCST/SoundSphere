﻿using System.Text.Json.Serialization;

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
        
        public IList<Authority> Authorities { get; set; } = new List<Authority>();
        
        [JsonIgnore] public IList<UserSong>? UserSongs { get; set; }
        
        [JsonIgnore] public IList<UserArtist>? UserArtists { get; set; }
    }
}