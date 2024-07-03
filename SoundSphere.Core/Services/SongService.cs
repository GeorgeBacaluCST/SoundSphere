using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IArtistRepository _artistRepository;

        public SongService(ISongRepository songRepository, IAlbumRepository albumRepository, IArtistRepository artistRepository) =>
            (_songRepository, _albumRepository, _artistRepository) = (songRepository, albumRepository, artistRepository);

        public IList<SongDto> GetAll() => _songRepository.GetAll().ToDtos();

        public SongDto GetById(Guid id) => _songRepository.GetById(id).ToDto();

        public SongDto Add(SongDto songDto)
        {
            Song songToAdd = songDto.ToEntity(_albumRepository, _artistRepository);
            _songRepository.LinkSongToAlbum(songToAdd);
            _songRepository.LinkSongToArtists(songToAdd);
            _songRepository.AddSongLink(songToAdd);
            _songRepository.AddUserSong(songToAdd);
            return _songRepository.Add(songToAdd).ToDto();
        }

        public SongDto UpdateById(SongDto songDto, Guid id) => _songRepository.UpdateById(songDto.ToEntity(_albumRepository, _artistRepository), id).ToDto();

        public SongDto DeleteById(Guid id) => _songRepository.DeleteById(id).ToDto();
    }
}