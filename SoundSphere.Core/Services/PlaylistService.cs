using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISongRepository _songRepository;

        public PlaylistService(IPlaylistRepository playlistRepository, IUserRepository userRepository, ISongRepository songRepository) =>
            (_playlistRepository, _userRepository, _songRepository) = (playlistRepository, userRepository, songRepository);

        public IList<PlaylistDto> GetAll() => _playlistRepository.GetAll().ToDtos();

        public PlaylistDto GetById(Guid id) => _playlistRepository.GetById(id).ToDto();

        public PlaylistDto Add(PlaylistDto playlistDto)
        {
            Playlist playlistToAdd = playlistDto.ToEntity(_userRepository, _songRepository);
            _playlistRepository.LinkPlaylistToUser(playlistToAdd);
            return _playlistRepository.Add(playlistToAdd).ToDto();
        }

        public PlaylistDto UpdateById(PlaylistDto playlistDto, Guid id) => _playlistRepository.UpdateById(playlistDto.ToEntity(_userRepository, _songRepository), id).ToDto();

        public PlaylistDto DeleteById(Guid id) => _playlistRepository.DeleteById(id).ToDto();
    }
}