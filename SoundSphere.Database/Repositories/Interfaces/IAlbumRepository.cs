using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IAlbumRepository
    {
        IList<Album> GetAll();

        Album GetById(Guid id);

        Album Add(Album album);

        Album UpdateById(Album album, Guid id);

        Album DeleteById(Guid id);

        void AddAlbumLink(Album album);
    }
}