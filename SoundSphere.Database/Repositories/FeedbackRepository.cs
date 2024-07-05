using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly SoundSphereDbContext _context;

        public FeedbackRepository(SoundSphereDbContext context) => _context = context;

        public IList<Feedback> GetAll() => _context.Feedbacks
            .Include(feedback => feedback.User)
            .Where(feedback => feedback.DeletedAt == null)
            .OrderBy(feedback => feedback.CreatedAt)
            .ToList();

        public Feedback GetById(Guid id) => _context.Feedbacks
            .Include(feedback => feedback.User)
            .Where(feedback => feedback.DeletedAt == null)
            .SingleOrDefault(feedback => feedback.Id == id)
            ?? throw new ResourceNotFoundException($"Feedback with id {id} not found");

        public Feedback Add(Feedback feedback)
        {
            if (feedback.Id == Guid.Empty) feedback.Id = Guid.NewGuid();
            feedback.CreatedAt = DateTime.Now;
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
            return feedback;
        }

        public Feedback UpdateById(Feedback feedback, Guid id)
        {
            Feedback feedbackToUpdate = GetById(id);
            feedbackToUpdate.Type = feedback.Type;
            feedbackToUpdate.Message = feedback.Message;
            if (_context.Entry(feedbackToUpdate).State == EntityState.Modified)
                feedbackToUpdate.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return feedbackToUpdate;
        }

        public Feedback DeleteById(Guid id)
        {
            Feedback feedbackToDelete = GetById(id);
            feedbackToDelete.DeletedAt = DateTime.Now;
            _context.SaveChanges();
            return feedbackToDelete;
        }

        public void LinkFeedbackToUser(Feedback feedback)
        {
            if (_context.Users.Find(feedback.User.Id) is User existingUser)
                feedback.User = _context.Attach(existingUser).Entity;
        }
    }
}