using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using static SoundSphere.Test.Mocks.SongMock;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Mocks
{
    public class PlaylistMock
    {
        private PlaylistMock() { }

        public static List<Playlist> GetPlaylists() => [GetPlaylist1(), GetPlaylist2()];

        public static List<PlaylistDto> GetPlaylistDtos() => GetPlaylists().Select(ToDto).ToList();

        public static Playlist GetPlaylist1() => new()
        {
            Id = Guid.Parse("239d050b-b59c-47e0-9e1a-ab5faf6f903e"),
            Title = "playlist_title1",
            User = GetUser1(),
            Songs = GetSongs1(),
            CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Playlist GetPlaylist2() => new()
        {
            Id = Guid.Parse("67b394ad-aeba-4804-be29-71fc4ebd37c8"),
            Title = "playlist_title2",
            User = GetUser2(),
            Songs = GetSongs2(),
            CreatedAt = new DateTime(2024, 7, 2, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static PlaylistDto GetPlaylistDto1() => ToDto(GetPlaylist1());

        public static PlaylistDto GetPlaylistDto2() => ToDto(GetPlaylist2());

        public static PlaylistDto ToDto(Playlist playlist) => new()
        {
            Id = playlist.Id,
            Title = playlist.Title,
            UserId = playlist.User.Id,
            SongsIds = playlist.Songs.Select(song => song.Id).ToList(),
            CreatedAt = playlist.CreatedAt,
            UpdatedAt = playlist.UpdatedAt,
            DeletedAt = playlist.DeletedAt
        };
    }
}