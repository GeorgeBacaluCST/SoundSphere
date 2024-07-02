using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly SoundSphereDbContext _context;

        public NotificationRepository(SoundSphereDbContext context) => _context = context;

        public IList<Notification> GetAll() => _context.Notifications
            .Include(notification => notification.Sender)
            .Include(notification => notification.Receiver)
            .Where(notification => notification.DeletedAt == null)
            .OrderBy(notification => notification.CreatedAt)
            .ToList();

        public Notification GetById(Guid id) => _context.Notifications
            .Include(notification => notification.Sender)
            .Include(notification => notification.Receiver)
            .Where(notification => notification.DeletedAt == null)
            .SingleOrDefault(notification => notification.Id == id)
            ?? throw new Exception($"Notification with id {id} not found");

        public Notification Add(Notification notification)
        {
            if (notification.Id == Guid.Empty) notification.Id = Guid.NewGuid();
            notification.CreatedAt = DateTime.Now;
            _context.Notifications.Add(notification);
            _context.SaveChanges();
            return notification;
        }

        public Notification UpdateById(Notification notification, Guid id)
        {
            Notification notificationToUpdate = GetById(id);
            notificationToUpdate.Type = notification.Type;
            notificationToUpdate.Message = notification.Message;
            notificationToUpdate.IsRead = notification.IsRead;
            if (_context.Entry(notificationToUpdate).State == EntityState.Modified)
                notificationToUpdate.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return notificationToUpdate;
        }

        public Notification DeleteById(Guid id)
        {
            Notification notificationToDelete = GetById(id);
            notificationToDelete.DeletedAt = DateTime.Now;
            _context.SaveChanges();
            return notificationToDelete;
        }

        public void LinkNotificationToSender(Notification notification)
        {
            if (_context.Users.Find(notification.SenderId) is User existingSender)
                notification.Sender = _context.Attach(existingSender).Entity;
        }

        public void LinkNotificationToReceiver(Notification notification)
        {
            if (_context.Users.Find(notification.ReceiverId) is User existingReceiver)
                notification.Receiver = _context.Attach(existingReceiver).Entity;
        }
    }
}