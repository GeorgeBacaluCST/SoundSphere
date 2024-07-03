﻿using AutoMapper;
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
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepository, IUserRepository userRepository, IMapper mapper) =>
            (_feedbackRepository, _userRepository, _mapper) = (feedbackRepository, userRepository, mapper);

        public IList<FeedbackDto> GetAll() => _feedbackRepository.GetAll().ToDtos(_mapper);

        public FeedbackDto GetById(Guid id) => _feedbackRepository.GetById(id).ToDto(_mapper);

        public FeedbackDto Add(FeedbackDto feedbackDto)
        {
            Feedback feedbackToAdd = feedbackDto.ToEntity(_userRepository, _mapper);
            _feedbackRepository.LinkFeedbackToUser(feedbackToAdd);
            return _feedbackRepository.Add(feedbackToAdd).ToDto(_mapper);
        }

        public FeedbackDto UpdateById(FeedbackDto feedbackDto, Guid id) => _feedbackRepository.UpdateById(feedbackDto.ToEntity(_userRepository, _mapper), id).ToDto(_mapper);

        public FeedbackDto DeleteById(Guid id) => _feedbackRepository.DeleteById(id).ToDto(_mapper);
    }
}