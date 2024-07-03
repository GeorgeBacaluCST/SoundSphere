using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository) => (_notificationRepository, _userRepository) = (notificationRepository, userRepository);

        public IList<NotificationDto> GetAll() => _notificationRepository.GetAll().ToDtos();

        public NotificationDto GetById(Guid id) => _notificationRepository.GetById(id).ToDto();

        public NotificationDto Add(NotificationDto notificationDto)
        {
            Notification notificationToAdd = notificationDto.ToEntity(_userRepository);
            _notificationRepository.LinkNotificationToSender(notificationToAdd);
            _notificationRepository.LinkNotificationToReceiver(notificationToAdd);
            return _notificationRepository.Add(notificationToAdd).ToDto();
        }

        public NotificationDto UpdateById(NotificationDto notificationDto, Guid id) => _notificationRepository.UpdateById(notificationDto.ToEntity(_userRepository), id).ToDto();

        public NotificationDto DeleteById(Guid id) => _notificationRepository.DeleteById(id).ToDto();
    }
}