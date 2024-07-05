using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Common
{
    public class NotificationDto : BaseEntity
    {
        [Required(ErrorMessage = "ID is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Sender ID is required")]
        public Guid SenderId { get; set; }

        [Required(ErrorMessage = "Receiver ID is required")]
        public Guid ReceiverId { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public NotificationType Type { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message can't be longer than 500 characters")]
        public string Message { get; set; } = null!;

        public bool IsRead { get; set; }
    }
}