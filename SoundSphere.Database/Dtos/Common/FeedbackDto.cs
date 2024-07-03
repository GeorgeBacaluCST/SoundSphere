using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Common
{
    public class FeedbackDto : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public FeedbackType Type { get; set; }

        public string Message { get; set; } = null!;
    }
}