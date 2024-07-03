using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IAlbumService
    {
        IList<AlbumDto> GetAll();

        AlbumDto GetById(Guid id);

        AlbumDto Add(AlbumDto albumDto);

        AlbumDto UpdateById(AlbumDto albumDto, Guid id);

        AlbumDto DeleteById(Guid id);
    }
}