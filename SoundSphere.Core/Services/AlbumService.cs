using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumService(IAlbumRepository albumRepository) => _albumRepository = albumRepository;

        public IList<AlbumDto> GetAll() => _albumRepository.GetAll().ToDtos();

        public AlbumDto GetById(Guid id) => _albumRepository.GetById(id).ToDto();

        public AlbumDto Add(AlbumDto albumDto)
        {
            Album albumToAdd = albumDto.ToEntity();
            _albumRepository.AddAlbumLink(albumToAdd);
            return _albumRepository.Add(albumToAdd).ToDto();
        }

        public AlbumDto UpdateById(AlbumDto albumDto, Guid id) => _albumRepository.UpdateById(albumDto.ToEntity(), id).ToDto();

        public AlbumDto DeleteById(Guid id) => _albumRepository.DeleteById(id).ToDto();
    }
}