using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository) => _feedbackRepository = feedbackRepository;

        public IList<Feedback> GetAll() => _feedbackRepository.GetAll();

        public Feedback GetById(Guid id) => _feedbackRepository.GetById(id);

        public Feedback Add(Feedback feedback)
        {
            _feedbackRepository.LinkFeedbackToUser(feedback);
            return _feedbackRepository.Add(feedback);
        }

        public Feedback UpdateById(Feedback feedback, Guid id) => _feedbackRepository.UpdateById(feedback, id);

        public Feedback DeleteById(Guid id) => _feedbackRepository.DeleteById(id);
    }
}