using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface INotificationService
    {
        IList<Notification> GetAll();

        Notification GetById(Guid id);

        Notification Add(Notification notification);

        Notification UpdateById(Notification notification, Guid id);

        Notification DeleteById(Guid id);
    }
}