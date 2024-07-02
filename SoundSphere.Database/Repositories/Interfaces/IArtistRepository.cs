using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IArtistRepository
    {
        IList<Artist> GetAll();

        Artist GetById(Guid id);

        Artist Add(Artist artist);

        Artist UpdateById(Artist artist, Guid id);

        Artist DeleteById(Guid id);

        void AddArtistLink(Artist artist);

        void AddUserArtist(Artist artist);
    }
}