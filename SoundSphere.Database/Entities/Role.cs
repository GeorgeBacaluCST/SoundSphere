namespace SoundSphere.Database.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        
        public RoleType Type { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }

    public enum RoleType { InvalidRole, Admin, Moderator, Listener }
}