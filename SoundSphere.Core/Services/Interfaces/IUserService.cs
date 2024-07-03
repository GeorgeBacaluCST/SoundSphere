using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IUserService
    {
        IList<User> GetAll();

        User GetById(Guid id);

        User Add(User user);

        User UpdateById(User user, Guid id);

        User DeleteById(Guid id);
    }
}