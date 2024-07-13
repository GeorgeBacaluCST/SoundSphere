using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Test.Mocks.NotificationMock;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Unit.Repositories
{
    public class NotificationRepositoryTest
    {
        private readonly Mock<DbSet<Notification>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly INotificationRepository _notificationRepository;

        private readonly Notification _notification1 = GetNotification1();
        private readonly Notification _notification2 = GetNotification2();
        private readonly List<Notification> _notifications = GetNotifications();
        private readonly User _user1 = GetUser1();
        private readonly User _user2 = GetUser2();

        public NotificationRepositoryTest()
        {
            IQueryable<Notification> queryableNotifications = _notifications.AsQueryable();
            _dbSetMock.As<IQueryable<Notification>>().Setup(mock => mock.Provider).Returns(queryableNotifications.Provider);
            _dbSetMock.As<IQueryable<Notification>>().Setup(mock => mock.Expression).Returns(queryableNotifications.Expression);
            _dbSetMock.As<IQueryable<Notification>>().Setup(mock => mock.ElementType).Returns(queryableNotifications.ElementType);
            _dbSetMock.As<IQueryable<Notification>>().Setup(mock => mock.GetEnumerator()).Returns(queryableNotifications.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Notifications).Returns(_dbSetMock.Object);
            _notificationRepository = new NotificationRepository(_dbContextMock.Object);
        }

        [Fact] public void GetAll_Test() => _notificationRepository.GetAll().Should().BeEquivalentTo(_notifications);

        [Fact] public void GetById_ValidId_Test() => _notificationRepository.GetById(ValidNotificationGuid).Should().BeEquivalentTo(_notification1);

        [Fact] public void GetById_InvalidId_Test() => _notificationRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(NotificationNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            Notification newNotification = new() { Sender = _user1, Receiver = _user2, Type = NotificationType.Music, Message = "new_notification_message" };
            Notification result = _notificationRepository.Add(newNotification);
            result.Should().BeEquivalentTo(newNotification, options => options.Excluding(notification => notification.Id).Excluding(notification => notification.CreatedAt));
            result.CreatedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Notification updatedNotification = new() { Sender = _user1, Receiver = _user2, Type = NotificationType.Social, Message = "updated_notification_message" };
            Mock<EntityEntry<Notification>> entryMock = new();
            entryMock.SetupProperty(mock => mock.State, EntityState.Modified);
            _dbContextMock.SetupProperty(mock => mock.Entry(It.IsAny<Notification>()), entryMock.Object);
            Notification result = _notificationRepository.UpdateById(updatedNotification, ValidNotificationGuid);
            result.Should().BeEquivalentTo(updatedNotification, options => options.Excluding(notification => notification.Id).Excluding(notification => notification.CreatedAt).Excluding(notification => notification.UpdatedAt));
            result.UpdatedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _notificationRepository
            .Invoking(repository => repository.UpdateById(It.IsAny<Notification>(), InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(NotificationNotFound, InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            Notification result = _notificationRepository.DeleteById(ValidNotificationGuid);
            result.DeletedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _notificationRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(NotificationNotFound, InvalidGuid));
    }
}