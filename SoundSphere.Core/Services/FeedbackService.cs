using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, IUserRepository userRepository) => (_feedbackRepository, _userRepository) = (feedbackRepository, userRepository);

        public IList<FeedbackDto> GetAll() => _feedbackRepository.GetAll().ToDtos();

        public FeedbackDto GetById(Guid id) => _feedbackRepository.GetById(id).ToDto();

        public FeedbackDto Add(FeedbackDto feedbackDto)
        {
            Feedback feedbackToAdd = feedbackDto.ToEntity(_userRepository);
            _feedbackRepository.LinkFeedbackToUser(feedbackToAdd);
            return _feedbackRepository.Add(feedbackToAdd).ToDto();
        }

        public FeedbackDto UpdateById(FeedbackDto feedbackDto, Guid id) => _feedbackRepository.UpdateById(feedbackDto.ToEntity(_userRepository), id).ToDto();

        public FeedbackDto DeleteById(Guid id) => _feedbackRepository.DeleteById(id).ToDto();
    }
}