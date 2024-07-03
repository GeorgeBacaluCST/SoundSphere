using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Mappings
{
    public static class PlaylistMappingExtensions
    {
        public static IList<PlaylistDto> ToDtos(this IList<Playlist> playlists) => playlists.Select(playlist => playlist.ToDto()).ToList();

        public static IList<Playlist> ToEntities(this IList<PlaylistDto> playlistDtos, IUserRepository userRepository, ISongRepository songReposiory) => playlistDtos.Select(playlistDto => playlistDto.ToEntity(userRepository, songReposiory)).ToList();

        public static PlaylistDto ToDto(this Playlist playlist) => new PlaylistDto
        {
            Id = playlist.Id,
            Title = playlist.Title,
            UserId = playlist.User.Id,
            SongsIds = playlist.Songs.Select(song => song.Id).ToList(),
            CreatedAt = playlist.CreatedAt,
            UpdatedAt = playlist.UpdatedAt,
            DeletedAt = playlist.DeletedAt
        };

        public static Playlist ToEntity(this PlaylistDto playlistDto, IUserRepository userRepository, ISongRepository songRepository) => new Playlist
        {
            Id = playlistDto.Id,
            Title = playlistDto.Title,
            User = userRepository.GetById(playlistDto.UserId),
            Songs = playlistDto.SongsIds.Select(songRepository.GetById).ToList(),
            CreatedAt = playlistDto.CreatedAt,
            UpdatedAt = playlistDto.UpdatedAt,
            DeletedAt = playlistDto.DeletedAt
        };
    }
}