using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using static SoundSphere.Database.Constants;
using static SoundSphere.Test.Mocks.NotificationMock;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Unit.Controllers
{
    public class NotificationControllerTest
    {
        private readonly Mock<INotificationService> _notificationServiceMock = new();
        private readonly NotificationController _notificationController;
        private readonly IMapper _mapper;

        private readonly NotificationDto _notificationDto1 = GetNotificationDto1();
        private readonly NotificationDto _notificationDto2 = GetNotificationDto2();
        private readonly List<NotificationDto> _notificationDtos = GetNotificationDtos();
        private readonly User _user1 = GetUser1();
        private readonly User _user2 = GetUser2();

        public NotificationControllerTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _notificationController = new(_notificationServiceMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _notificationServiceMock.Setup(mock => mock.GetAll()).Returns(_notificationDtos);
            OkObjectResult? result = _notificationController.GetAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_notificationDtos);
        }

        [Fact] public void GetById_Test()
        {
            _notificationServiceMock.Setup(mock => mock.GetById(ValidNotificationGuid)).Returns(_notificationDto1);
            OkObjectResult? result = _notificationController.GetById(ValidNotificationGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_notificationDto1);
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
            _notificationServiceMock.Setup(mock => mock.Add(It.IsAny<NotificationDto>())).Returns(newNotificationDto);
            CreatedAtActionResult? result = _notificationController.GetById(ValidNotificationGuid) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(newNotificationDto);
        }

        [Fact] public void UpdateById_Test()
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
            _notificationServiceMock.Setup(mock => mock.UpdateById(It.IsAny<NotificationDto>(), ValidNotificationGuid)).Returns(updatedNotificationDto);
            OkObjectResult? result = _notificationController.UpdateById(updatedNotificationDto, ValidNotificationGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(updatedNotificationDto);
        }

        [Fact] public void DeleteById_Test()
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
            _notificationServiceMock.Setup(mock => mock.DeleteById(ValidNotificationGuid)).Returns(deletedNotificationDto);
            OkObjectResult? result = _notificationController.DeleteById(ValidNotificationGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(deletedNotificationDto);
        }
    }
}