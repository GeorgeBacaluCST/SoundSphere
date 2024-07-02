﻿using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        IList<Feedback> GetAll();

        Feedback GetById(Guid id);

        Feedback Add(Feedback feedback);

        Feedback UpdateById(Feedback feedback, Guid id);

        Feedback DeleteById(Guid id);

        void LinkFeedbackToUser(Feedback feedback);
    }
}