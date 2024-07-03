using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository) => _notificationRepository = notificationRepository;

        public IList<Notification> GetAll() => _notificationRepository.GetAll();

        public Notification GetById(Guid id) => _notificationRepository.GetById(id);

        public Notification Add(Notification notification)
        {
            _notificationRepository.LinkNotificationToSender(notification);
            _notificationRepository.LinkNotificationToReceiver(notification);
            return _notificationRepository.Add(notification);
        }

        public Notification UpdateById(Notification notification, Guid id) => _notificationRepository.UpdateById(notification, id);

        public Notification DeleteById(Guid id) => _notificationRepository.DeleteById(id);
    }
}