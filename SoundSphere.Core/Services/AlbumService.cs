using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumService(IAlbumRepository albumRepository) => _albumRepository = albumRepository;

        public IList<Album> GetAll() => _albumRepository.GetAll();

        public Album GetById(Guid id) => _albumRepository.GetById(id);

        public Album Add(Album album)
        {
            _albumRepository.AddAlbumLink(album);
            return _albumRepository.Add(album);
        }

        public Album UpdateById(Album album, Guid id) => _albumRepository.UpdateById(album, id);

        public Album DeleteById(Guid id) => _albumRepository.DeleteById(id);
    }
}