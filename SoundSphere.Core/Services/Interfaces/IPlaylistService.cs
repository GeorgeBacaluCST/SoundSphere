using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IPlaylistService
    {
        IList<Playlist> GetAll();

        Playlist GetById(Guid id);

        Playlist Add(Playlist playlist);

        Playlist UpdateById(Playlist playlist, Guid id);

        Playlist DeleteById(Guid id);
    }
}