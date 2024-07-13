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
using static SoundSphere.Test.Mocks.FeedbackMock;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Unit.Controllers
{
    public class FeedbackControllerTest
    {
        private readonly Mock<IFeedbackService> _feedbackServiceMock = new();
        private readonly FeedbackController _feedbackController;
        private readonly IMapper _mapper;

        private readonly FeedbackDto _feedbackDto1 = GetFeedbackDto1();
        private readonly FeedbackDto _feedbackDto2 = GetFeedbackDto2();
        private readonly List<FeedbackDto> _feedbackDtos = GetFeedbackDtos();
        private readonly User _user1 = GetUser1();

        public FeedbackControllerTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _feedbackController = new(_feedbackServiceMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _feedbackServiceMock.Setup(mock => mock.GetAll()).Returns(_feedbackDtos);
            OkObjectResult? result = _feedbackController.GetAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_feedbackDtos);
        }

        [Fact] public void GetById_Test()
        {
            _feedbackServiceMock.Setup(mock => mock.GetById(ValidFeedbackGuid)).Returns(_feedbackDto1);
            OkObjectResult? result = _feedbackController.GetById(ValidFeedbackGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_feedbackDto1);
        }

        [Fact] public void Add_Test()
        {
            Feedback newFeedback = new()
            {
                Id = ValidFeedbackGuid,
                User = _user1,
                Type = FeedbackType.Issue,
                Message = "new_feedback_message",
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0)
            };
            FeedbackDto newFeedbackDto = newFeedback.ToDto(_mapper);
            _feedbackServiceMock.Setup(mock => mock.Add(It.IsAny<FeedbackDto>())).Returns(newFeedbackDto);
            CreatedAtActionResult? result = _feedbackController.GetById(ValidFeedbackGuid) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(newFeedbackDto);
        }

        [Fact] public void UpdateById_Test()
        {
            Feedback updatedFeedback = new()
            {
                Id = ValidFeedbackGuid,
                User = _user1,
                Type = FeedbackType.Optimization,
                Message = "updated_feedback_message",
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
                UpdatedAt = new DateTime(2024, 7, 2, 0, 0, 0)
            };
            FeedbackDto updatedFeedbackDto = updatedFeedback.ToDto(_mapper);
            _feedbackServiceMock.Setup(mock => mock.UpdateById(It.IsAny<FeedbackDto>(), ValidFeedbackGuid)).Returns(updatedFeedbackDto);
            OkObjectResult? result = _feedbackController.UpdateById(updatedFeedbackDto, ValidFeedbackGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(updatedFeedbackDto);
        }

        [Fact] public void DeleteById_Test()
        {
            Feedback deletedFeedback = new()
            {
                Id = ValidFeedbackGuid,
                User = _user1,
                Type = FeedbackType.Improvement,
                Message = "deleted_feedback_message",
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
                UpdatedAt = new DateTime(2024, 7, 2, 0, 0, 0),
                DeletedAt = new DateTime(2024, 7, 3, 0, 0, 0)
            };
            FeedbackDto deletedFeedbackDto = deletedFeedback.ToDto(_mapper);
            _feedbackServiceMock.Setup(mock => mock.DeleteById(ValidFeedbackGuid)).Returns(deletedFeedbackDto);
            OkObjectResult? result = _feedbackController.DeleteById(ValidFeedbackGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(deletedFeedbackDto);
        }
    }
}