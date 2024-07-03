using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface ISongService
    {
        IList<SongDto> GetAll();

        SongDto GetById(Guid id);

        SongDto Add(SongDto songDto);

        SongDto UpdateById(SongDto songDto, Guid id);

        SongDto DeleteById(Guid id);
    }
}