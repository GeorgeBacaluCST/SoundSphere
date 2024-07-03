using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IArtistRepository artistRepository) => _artistRepository = artistRepository;

        public IList<Artist> GetAll() => _artistRepository.GetAll();

        public Artist GetById(Guid id) => _artistRepository.GetById(id);

        public Artist Add(Artist artist)
        {
            _artistRepository.AddArtistLink(artist);
            _artistRepository.AddUserArtist(artist);
            return _artistRepository.Add(artist);
        }

        public Artist UpdateById(Artist artist, Guid id) => _artistRepository.UpdateById(artist, id);

        public Artist DeleteById(Guid id) => _artistRepository.DeleteById(id);
    }
}