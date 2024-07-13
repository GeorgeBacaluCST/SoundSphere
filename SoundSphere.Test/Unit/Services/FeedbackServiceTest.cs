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
using static SoundSphere.Test.Mocks.FeedbackMock;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Unit.Services
{
    public class FeedbackServiceTest
    {
        private readonly Mock<IFeedbackRepository> _feedbackRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly IFeedbackService _feedbackService;
        private readonly IMapper _mapper;

        private readonly Feedback _feedback1 = GetFeedback1();
        private readonly Feedback _feedback2 = GetFeedback2();
        private readonly List<Feedback> _feedbacks = GetFeedbacks();
        private readonly FeedbackDto _feedbackDto1 = GetFeedbackDto1();
        private readonly FeedbackDto _feedbackDto2 = GetFeedbackDto2();
        private readonly List<FeedbackDto> _feedbackDtos = GetFeedbackDtos();
        private readonly User _user1 = GetUser1();

        public FeedbackServiceTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _feedbackService = new FeedbackService(_feedbackRepositoryMock.Object, _userRepositoryMock.Object, _mapper);
        }

        [Fact] public void GetAll_Test()
        {
            _feedbackRepositoryMock.Setup(mock => mock.GetAll()).Returns(_feedbacks);
            _feedbackService.GetAll().Should().BeEquivalentTo(_feedbackDtos);
        }

        [Fact] public void GetById_ValidId_Test()
        {
            _feedbackRepositoryMock.Setup(mock => mock.GetById(ValidFeedbackGuid)).Returns(_feedback1);
            _feedbackService.GetById(ValidFeedbackGuid).Should().BeEquivalentTo(_feedbackDto1);
        }

        [Fact] public void GetById_InvalidId_Test()
        {
            _feedbackRepositoryMock.Setup(mock => mock.GetById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(FeedbackNotFound, InvalidGuid)));
            _feedbackService.Invoking(service => service.GetById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(FeedbackNotFound, InvalidGuid));
            _feedbackRepositoryMock.Verify(mock => mock.GetById(InvalidGuid));
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
            _userRepositoryMock.Setup(mock => mock.GetById(ValidUserGuid)).Returns(_user1);
            _feedbackRepositoryMock.Setup(mock => mock.Add(It.IsAny<Feedback>())).Returns(newFeedback);
            _feedbackService.Add(newFeedbackDto).Should().BeEquivalentTo(newFeedbackDto);
            _feedbackRepositoryMock.Verify(mock => mock.Add(It.IsAny<Feedback>()));
        }

        [Fact] public void UpdateById_ValidId_Test()
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
            _feedbackRepositoryMock.Setup(mock => mock.UpdateById(It.IsAny<Feedback>(), ValidFeedbackGuid)).Returns(updatedFeedback);
            _feedbackService.UpdateById(updatedFeedbackDto, ValidFeedbackGuid).Should().BeEquivalentTo(updatedFeedbackDto);
            _feedbackRepositoryMock.Verify(mock => mock.UpdateById(It.IsAny<Feedback>(), ValidFeedbackGuid));
        }

        [Fact] public void UpdateById_InvalidId_Test()
        {
            FeedbackDto invalidFeedbackDto = new()
            {
                Id = InvalidGuid,
                UserId = Guid.Empty,
                Type = FeedbackType.InvalidFeedbackType,
                Message = "invalid_feedback_message"
            };
            _feedbackRepositoryMock.Setup(mock => mock.UpdateById(It.IsAny<Feedback>(), InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(FeedbackNotFound, InvalidGuid)));
            _feedbackService.Invoking(service => service.UpdateById(invalidFeedbackDto, InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(FeedbackNotFound, InvalidGuid));
            _feedbackRepositoryMock.Verify(mock => mock.UpdateById(It.IsAny<Feedback>(), InvalidGuid));
        }

        [Fact] public void DeleteById_ValidId_Test()
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
            _feedbackRepositoryMock.Setup(mock => mock.DeleteById(ValidFeedbackGuid)).Returns(deletedFeedback);
            _feedbackService.DeleteById(ValidFeedbackGuid).Should().BeEquivalentTo(deletedFeedbackDto);
            _feedbackRepositoryMock.Verify(mock => mock.DeleteById(ValidFeedbackGuid));
        }

        [Fact] public void DeleteById_InvalidId_Test()
        {
            _feedbackRepositoryMock.Setup(mock => mock.DeleteById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(FeedbackNotFound, InvalidGuid)));
            _feedbackService.Invoking(service => service.DeleteById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(FeedbackNotFound, InvalidGuid));
            _feedbackRepositoryMock.Verify(mock => mock.DeleteById(InvalidGuid));
        }
    }
}