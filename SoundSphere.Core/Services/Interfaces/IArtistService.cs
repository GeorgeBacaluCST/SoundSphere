using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IArtistService
    {
        IList<ArtistDto> GetAll();

        ArtistDto GetById(Guid id);

        ArtistDto Add(ArtistDto artistDto);

        ArtistDto UpdateById(ArtistDto artistDto, Guid id);

        ArtistDto DeleteById(Guid id);
    }
}