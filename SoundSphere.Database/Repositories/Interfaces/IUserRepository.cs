using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IList<User> GetAll();

        User GetById(Guid id);

        User Add(User user);

        User UpdateById(User user, Guid id);

        User DeleteById(Guid id);

        void LinkUserToRole(User user);

        void LinkUserToAuthorities(User user);

        void AddUserArtist(User user);

        void AddUserSong(User user);
    }
}