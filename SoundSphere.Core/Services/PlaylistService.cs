using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistService(IPlaylistRepository playlistRepository) => _playlistRepository = playlistRepository;

        public IList<Playlist> GetAll() => _playlistRepository.GetAll();

        public Playlist GetById(Guid id) => _playlistRepository.GetById(id);

        public Playlist Add(Playlist playlist)
        {
            _playlistRepository.LinkPlaylistToUser(playlist);
            return _playlistRepository.Add(playlist);
        }

        public Playlist UpdateById(Playlist playlist, Guid id) => _playlistRepository.UpdateById(playlist, id);

        public Playlist DeleteById(Guid id) => _playlistRepository.DeleteById(id);
    }
}