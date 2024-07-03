using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Mappings
{
    public static class FeedbackMappingExtensions
    {
        public static IList<FeedbackDto> ToDtos(this IList<Feedback> feedbacks) => feedbacks.Select(feedback => feedback.ToDto()).ToList();

        public static IList<Feedback> ToEntities(this IList<FeedbackDto> feedbackDtos, IUserRepository userRepository) => feedbackDtos.Select(feedbackDto => feedbackDto.ToEntity(userRepository)).ToList();

        public static FeedbackDto ToDto(this Feedback feedback) => new FeedbackDto
        {
            Id = feedback.Id,
            UserId = feedback.User.Id,
            Type = feedback.Type,
            Message = feedback.Message,
            CreatedAt = feedback.CreatedAt,
            UpdatedAt = feedback.UpdatedAt,
            DeletedAt = feedback.DeletedAt
        };

        public static Feedback ToEntity(this FeedbackDto feedbackDto, IUserRepository userRepository) => new Feedback
        {
            Id = feedbackDto.Id,
            User = userRepository.GetById(feedbackDto.UserId),
            Type = feedbackDto.Type,
            Message = feedbackDto.Message,
            CreatedAt = feedbackDto.CreatedAt,
            UpdatedAt = feedbackDto.UpdatedAt,
            DeletedAt = feedbackDto.DeletedAt
        };
    }
}