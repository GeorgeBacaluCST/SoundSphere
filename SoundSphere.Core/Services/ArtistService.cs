using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IArtistRepository artistRepository) => _artistRepository = artistRepository;

        public IList<ArtistDto> GetAll() => _artistRepository.GetAll().ToDtos();

        public ArtistDto GetById(Guid id) => _artistRepository.GetById(id).ToDto();

        public ArtistDto Add(ArtistDto artistDto)
        {
            Artist artistToAdd = artistDto.ToEntity();
            _artistRepository.AddArtistLink(artistToAdd);
            _artistRepository.AddUserArtist(artistToAdd);
            return _artistRepository.Add(artistToAdd).ToDto();
        }

        public ArtistDto UpdateById(ArtistDto artistDto, Guid id) => _artistRepository.UpdateById(artistDto.ToEntity(), id).ToDto();

        public ArtistDto DeleteById(Guid id) => _artistRepository.DeleteById(id).ToDto();
    }
}