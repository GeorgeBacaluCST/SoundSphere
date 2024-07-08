using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Mocks
{
    public class NotificationMock
    {
        private NotificationMock() { }

        public static IList<Notification> GetNotifications() => [GetNotification1(), GetNotification2()];

        public static IList<NotificationDto> GetNotificationDtos() => GetNotifications().Select(ToDto).ToList();

        public static Notification GetNotification1() => new()
        {
            Id = Guid.Parse("7e221fa3-2c22-4573-bf21-cd1d6696b576"),
            SenderId = Guid.Parse("0a9e546f-38b4-4dbf-a482-24a82169890e"),
            ReceiverId = Guid.Parse("7eb88892-549b-4cae-90be-c52088354643"),
            Sender = GetUser1(),
            Receiver = GetUser2(),
            Type = NotificationType.Music,
            Message = "notification_message1",
            IsRead = false,
            CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Notification GetNotification2() => new()
        {
            Id = Guid.Parse("1d23fa22-3455-407b-9371-c42d56001de7"),
            SenderId = Guid.Parse("7eb88892-549b-4cae-90be-c52088354643"),
            ReceiverId = Guid.Parse("0a9e546f-38b4-4dbf-a482-24a82169890e"),
            Sender = GetUser2(),
            Receiver = GetUser1(),
            Type = NotificationType.Social,
            Message = "notification_message2",
            IsRead = false,
            CreatedAt = new DateTime(2024, 7, 2, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static NotificationDto GetNotificationDto1() => ToDto(GetNotification1());

        public static NotificationDto GetNotificationDto2() => ToDto(GetNotification2());

        public static NotificationDto ToDto(Notification notification) => new()
        {
            Id = notification.Id,
            SenderId = notification.SenderId,
            ReceiverId = notification.ReceiverId,
            Type = notification.Type,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt,
            UpdatedAt = notification.UpdatedAt,
            DeletedAt = notification.DeletedAt
        };
    }
}