using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Mappings
{
    public static class NotificationMappingExtensions
    {
        public static IList<NotificationDto> ToDtos(this IList<Notification> notifications) => notifications.Select(notification => notification.ToDto()).ToList();

        public static IList<Notification> ToEntities(this IList<NotificationDto> notificationDtos, IUserRepository userRepository) => notificationDtos.Select(notificationDto => notificationDto.ToEntity(userRepository)).ToList();

        public static NotificationDto ToDto(this Notification notification) => new NotificationDto
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

        public static Notification ToEntity(this NotificationDto notificationDto, IUserRepository userRepository) => new Notification
        {
            Id = notificationDto.Id,
            Sender = userRepository.GetById(notificationDto.SenderId),
            Receiver = userRepository.GetById(notificationDto.ReceiverId),
            Type = notificationDto.Type,
            Message = notificationDto.Message,
            IsRead = notificationDto.IsRead,
            CreatedAt = notificationDto.CreatedAt,
            UpdatedAt = notificationDto.UpdatedAt,
            DeletedAt = notificationDto.DeletedAt
        };
    }
}