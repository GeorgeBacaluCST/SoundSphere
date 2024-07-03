using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;

        public SongService(ISongRepository songRepository) => _songRepository = songRepository;

        public IList<Song> GetAll() => _songRepository.GetAll();

        public Song GetById(Guid id) => _songRepository.GetById(id);

        public Song Add(Song song)
        {
            _songRepository.LinkSongToAlbum(song);
            _songRepository.LinkSongToArtists(song);
            _songRepository.AddSongLink(song);
            _songRepository.AddUserSong(song);
            return _songRepository.Add(song);
        }

        public Song UpdateById(Song song, Guid id) => _songRepository.UpdateById(song, id);

        public Song DeleteById(Guid id) => _songRepository.DeleteById(id);
    }
}