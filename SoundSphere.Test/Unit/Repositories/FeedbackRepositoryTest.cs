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
using static SoundSphere.Test.Mocks.FeedbackMock;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Unit.Repositories
{
    public class FeedbackRepositoryTest
    {
        private readonly Mock<DbSet<Feedback>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IFeedbackRepository _feedbackRepository;

        private readonly Feedback _feedback1 = GetFeedback1();
        private readonly Feedback _feedback2 = GetFeedback2();
        private readonly List<Feedback> _feedbacks = GetFeedbacks();
        private readonly User _user1 = GetUser1();

        public FeedbackRepositoryTest()
        {
            IQueryable<Feedback> queryableFeedbacks = _feedbacks.AsQueryable();
            _dbSetMock.As<IQueryable<Feedback>>().Setup(mock => mock.Provider).Returns(queryableFeedbacks.Provider);
            _dbSetMock.As<IQueryable<Feedback>>().Setup(mock => mock.Expression).Returns(queryableFeedbacks.Expression);
            _dbSetMock.As<IQueryable<Feedback>>().Setup(mock => mock.ElementType).Returns(queryableFeedbacks.ElementType);
            _dbSetMock.As<IQueryable<Feedback>>().Setup(mock => mock.GetEnumerator()).Returns(queryableFeedbacks.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Feedbacks).Returns(_dbSetMock.Object);
            _feedbackRepository = new FeedbackRepository(_dbContextMock.Object);
        }

        [Fact] public void GetAll_Test() => _feedbackRepository.GetAll().Should().BeEquivalentTo(_feedbacks);

        [Fact] public void GetById_ValidId_Test() => _feedbackRepository.GetById(ValidFeedbackGuid).Should().BeEquivalentTo(_feedback1);

        [Fact] public void GetById_InvalidId_Test() => _feedbackRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(FeedbackNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            Feedback newFeedback = new() { User = _user1, Type = FeedbackType.Issue, Message = "new_feedback_message" };
            Feedback result = _feedbackRepository.Add(newFeedback);
            result.Should().BeEquivalentTo(newFeedback, options => options.Excluding(feedback => feedback.Id).Excluding(feedback => feedback.CreatedAt));
            result.CreatedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Feedback updatedFeedback = new() { User = _user1, Type = FeedbackType.Optimization, Message = "updated_feedback_message" };
            Mock<EntityEntry<Feedback>> entryMock = new();
            entryMock.SetupProperty(mock => mock.State, EntityState.Modified);
            _dbContextMock.SetupProperty(mock => mock.Entry(It.IsAny<Feedback>()), entryMock.Object);
            Feedback result = _feedbackRepository.UpdateById(updatedFeedback, ValidFeedbackGuid);
            result.Should().BeEquivalentTo(updatedFeedback, options => options.Excluding(feedback => feedback.Id).Excluding(feedback => feedback.CreatedAt).Excluding(feedback => feedback.UpdatedAt));
            result.UpdatedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _feedbackRepository
            .Invoking(repository => repository.UpdateById(It.IsAny<Feedback>(), InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(FeedbackNotFound, InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            Feedback result = _feedbackRepository.DeleteById(ValidFeedbackGuid);
            result.DeletedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _feedbackRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(FeedbackNotFound, InvalidGuid));
    }
}