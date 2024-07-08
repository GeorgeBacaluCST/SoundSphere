using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Mocks
{
    public class FeedbackMock
    {
        private FeedbackMock() { }

        public static IList<Feedback> GetFeedbacks() => [GetFeedback1(), GetFeedback2()];

        public static IList<FeedbackDto> GetFeedbackDtos() => GetFeedbacks().Select(ToDto).ToList();

        public static Feedback GetFeedback1() => new()
        {
            Id = Guid.Parse("83061e8c-3403-441a-8be5-867ed1f4a86b"),
            User = GetUser1(),
            Type = FeedbackType.Issue,
            Message = "feedback_message1",
            CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Feedback GetFeedback2() => new()
        {
            Id = Guid.Parse("bf823996-d2ce-4616-a6b2-f7347f05c6aa"),
            User = GetUser2(),
            Type = FeedbackType.Optimization,
            Message = "feedback_message2",
            CreatedAt = new DateTime(2024, 7, 2, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static FeedbackDto GetFeedbackDto1() => ToDto(GetFeedback1());

        public static FeedbackDto GetFeedbackDto2() => ToDto(GetFeedback2());

        public static FeedbackDto ToDto(Feedback feedback) => new()
        {
            Id = feedback.Id,
            UserId = feedback.User.Id,
            Type = feedback.Type,
            Message = feedback.Message,
            CreatedAt = feedback.CreatedAt,
            UpdatedAt = feedback.UpdatedAt,
            DeletedAt = feedback.DeletedAt
        };
    }
}