using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IFeedbackService
    {
        IList<Feedback> GetAll();

        Feedback GetById(Guid id);

        Feedback Add(Feedback feedback);

        Feedback UpdateById(Feedback feedback, Guid id);

        Feedback DeleteById(Guid id);
    }
}