using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Common
{
    public class NotificationDto : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid SenderId { get; set; }

        public Guid ReceiverId { get; set; }

        public NotificationType Type { get; set; }

        public string Message { get; set; } = null!;

        public bool IsRead { get; set; }
    }
}