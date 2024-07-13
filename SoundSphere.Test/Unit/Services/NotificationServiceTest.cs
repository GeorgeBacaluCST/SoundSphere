using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Core.Services;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Test.Mocks.NotificationMock;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Unit.Services
{
    public class NotificationServiceTest
    {
        private readonly Mock<INotificationRepository> _notificationRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        private readonly Notification _notification1 = GetNotification1();
        private readonly Notification _notification2 = GetNotification2();
        private readonly List<Notification> _notifications = GetNotifications();
        private readonly NotificationDto _notificationDto1 = GetNotificationDto1();
        private readonly NotificationDto _notificationDto2 = GetNotificationDto2();
        private readonly List<NotificationDto> _notificationDtos = GetNotificationDtos();
        private readonly User _user1 = GetUser1();
        private readonly User _user2 = GetUser2();

        public NotificationServiceTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _notificationService = new NotificationService(_notificationRepositoryMock.Object, _userRepositoryMock.Object, _mapper);
        }

        [Fact] public void GetAll_Test()
        {
            _notificationRepositoryMock.Setup(mock => mock.GetAll()).Returns(_notifications);
            _notificationService.GetAll().Should().BeEquivalentTo(_notificationDtos);
        }

        [Fact] public void GetById_ValidId_Test()
        {
            _notificationRepositoryMock.Setup(mock => mock.GetById(ValidNotificationGuid)).Returns(_notification1);
            _notificationService.GetById(ValidNotificationGuid).Should().BeEquivalentTo(_notificationDto1);
        }

        [Fact] public void GetById_InvalidId_Test()
        {
            _notificationRepositoryMock.Setup(mock => mock.GetById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(NotificationNotFound, InvalidGuid)));
            _notificationService.Invoking(service => service.GetById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(NotificationNotFound, InvalidGuid));
            _notificationRepositoryMock.Verify(mock => mock.GetById(InvalidGuid));
        }

        [Fact] public void Add_Test()
        {
            Notification newNotification = new()
            {
                Id = ValidNotificationGuid,
                Sender = _user1,
                Receiver = _user2,
                Type = NotificationType.Music,
                Message = "new_notification_message",
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0)
            };
            NotificationDto newNotificationDto = newNotification.ToDto(_mapper);
            _userRepositoryMock.Setup(mock => mock.GetById(ValidUserGuid)).Returns(_user1);
            _notificationRepositoryMock.Setup(mock => mock.Add(It.IsAny<Notification>())).Returns(newNotification);
            _notificationService.Add(newNotificationDto).Should().BeEquivalentTo(newNotificationDto);
            _notificationRepositoryMock.Verify(mock => mock.Add(It.IsAny<Notification>()));
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Notification updatedNotification = new()
            {
                Id = ValidNotificationGuid,
                Sender = _user1,
                Receiver = _user2,
                Type = NotificationType.Social,
                Message = "updated_notification_message",
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
                UpdatedAt = new DateTime(2024, 7, 2, 0, 0, 0)
            };
            NotificationDto updatedNotificationDto = updatedNotification.ToDto(_mapper);
            _notificationRepositoryMock.Setup(mock => mock.UpdateById(It.IsAny<Notification>(), ValidNotificationGuid)).Returns(updatedNotification);
            _notificationService.UpdateById(updatedNotificationDto, ValidNotificationGuid).Should().BeEquivalentTo(updatedNotificationDto);
            _notificationRepositoryMock.Verify(mock => mock.UpdateById(It.IsAny<Notification>(), ValidNotificationGuid));
        }

        [Fact] public void UpdateById_InvalidId_Test()
        {
            NotificationDto invalidNotificationDto = new()
            {
                Id = InvalidGuid,
                SenderId = Guid.Empty,
                ReceiverId = Guid.Empty,
                Type = NotificationType.InvalidNotificationType,
                Message = "invalid_notification_message"
            };
            _notificationRepositoryMock.Setup(mock => mock.UpdateById(It.IsAny<Notification>(), InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(NotificationNotFound, InvalidGuid)));
            _notificationService.Invoking(service => service.UpdateById(invalidNotificationDto, InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(NotificationNotFound, InvalidGuid));
            _notificationRepositoryMock.Verify(mock => mock.UpdateById(It.IsAny<Notification>(), InvalidGuid));
        }

        [Fact] public void DeleteById_ValidId_Test()
        {
            Notification deletedNotification = new()
            {
                Id = ValidNotificationGuid,
                Sender = _user1,
                Receiver = _user2,
                Type = NotificationType.Account,
                Message = "deleted_notification_message",
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
                UpdatedAt = new DateTime(2024, 7, 2, 0, 0, 0),
                DeletedAt = new DateTime(2024, 7, 3, 0, 0, 0)
            };
            NotificationDto deletedNotificationDto = deletedNotification.ToDto(_mapper);
            _notificationRepositoryMock.Setup(mock => mock.DeleteById(ValidNotificationGuid)).Returns(deletedNotification);
            _notificationService.DeleteById(ValidNotificationGuid).Should().BeEquivalentTo(deletedNotificationDto);
            _notificationRepositoryMock.Verify(mock => mock.DeleteById(ValidNotificationGuid));
        }

        [Fact] public void DeleteById_InvalidId_Test()
        {
            _notificationRepositoryMock.Setup(mock => mock.DeleteById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(NotificationNotFound, InvalidGuid)));
            _notificationService.Invoking(service => service.DeleteById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(NotificationNotFound, InvalidGuid));
            _notificationRepositoryMock.Verify(mock => mock.DeleteById(InvalidGuid));
        }
    }
}