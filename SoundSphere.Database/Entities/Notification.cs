namespace SoundSphere.Database.Entities
{
    public class Notification : BaseEntity
    {
        public Guid Id { get; set; }
        
        public Guid SenderId { get; set; }
        
        public Guid ReceiverId { get; set; }
        
        public User Sender { get; set; } = null!;
        
        public User Receiver { get; set; } = null!;
        
        public NotificationType Type { get; set; }
        
        public string Message { get; set; } = null!;
        
        public bool IsRead { get; set; }
    }

    public enum NotificationType { InvalidNotificationType, Music, Social, Account, System }
}