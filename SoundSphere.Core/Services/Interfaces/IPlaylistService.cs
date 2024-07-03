using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IPlaylistService
    {
        IList<PlaylistDto> GetAll();

        PlaylistDto GetById(Guid id);

        PlaylistDto Add(PlaylistDto playlistDto);

        PlaylistDto UpdateById(PlaylistDto playlistDto, Guid id);

        PlaylistDto DeleteById(Guid id);
    }
}