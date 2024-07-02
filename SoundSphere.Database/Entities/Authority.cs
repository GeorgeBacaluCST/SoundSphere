using System.Text.Json.Serialization;

namespace SoundSphere.Database.Entities
{
    public class Authority
    {
        public Guid Id { get; set; }
        
        public AuthorityType Type { get; set; }
        
        [JsonIgnore] public IList<User>? Users { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }

    public enum AuthorityType { InvalidAuthority, Create, Read, Update, Delete }
}