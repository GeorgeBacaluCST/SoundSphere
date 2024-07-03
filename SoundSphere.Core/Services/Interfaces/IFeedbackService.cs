using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IFeedbackService
    {
        IList<FeedbackDto> GetAll();

        FeedbackDto GetById(Guid id);

        FeedbackDto Add(FeedbackDto feedbackDto);

        FeedbackDto UpdateById(FeedbackDto feedbackDto, Guid id);

        FeedbackDto DeleteById(Guid id);
    }
}