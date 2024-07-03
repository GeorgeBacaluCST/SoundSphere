using AutoMapper;
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
        private readonly IMapper _mapper;

        public SongService(ISongRepository songRepository, IAlbumRepository albumRepository, IArtistRepository artistRepository, IMapper mapper) =>
            (_songRepository, _albumRepository, _artistRepository, _mapper) = (songRepository, albumRepository, artistRepository, mapper);

        public IList<SongDto> GetAll() => _songRepository.GetAll().ToDtos(_mapper);

        public SongDto GetById(Guid id) => _songRepository.GetById(id).ToDto(_mapper);

        public SongDto Add(SongDto songDto)
        {
            Song songToAdd = songDto.ToEntity(_albumRepository, _artistRepository, _mapper);
            _songRepository.LinkSongToAlbum(songToAdd);
            _songRepository.LinkSongToArtists(songToAdd);
            _songRepository.AddSongLink(songToAdd);
            _songRepository.AddUserSong(songToAdd);
            return _songRepository.Add(songToAdd).ToDto(_mapper);
        }

        public SongDto UpdateById(SongDto songDto, Guid id) => _songRepository.UpdateById(songDto.ToEntity(_albumRepository, _artistRepository, _mapper), id).ToDto(_mapper);

        public SongDto DeleteById(Guid id) => _songRepository.DeleteById(id).ToDto(_mapper);
    }
}