using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Mappings
{
    public static class SongMappingExtensions
    {
        public static IList<SongDto> ToDtos(this IList<Song> songs) => songs.Select(song => song.ToDto()).ToList();

        public static IList<Song> ToEntities(this IList<SongDto> songDtos, IAlbumRepository albumRepository, IArtistRepository artistRepository) => songDtos.Select(songDto => songDto.ToEntity(albumRepository, artistRepository)).ToList();

        public static SongDto ToDto(this Song song) => new SongDto
        {
            Id = song.Id,
            Title = song.Title,
            ImageUrl = song.ImageUrl,
            Genre = song.Genre,
            ReleaseDate = song.ReleaseDate,
            DurationSeconds = song.DurationSeconds,
            AlbumId = song.Album.Id,
            ArtistsIds = song.Artists.Select(artist => artist.Id).ToList(),
            SimilarSongsIds = song.SimilarSongs.Select(songLink => songLink.SimilarSongId).ToList(),
            CreatedAt = song.CreatedAt,
            UpdatedAt = song.UpdatedAt,
            DeletedAt = song.DeletedAt
        };

        public static Song ToEntity(this SongDto songDto, IAlbumRepository albumRepository, IArtistRepository artistRepository) => new Song
        {
            Id = songDto.Id,
            Title = songDto.Title,
            ImageUrl = songDto.ImageUrl,
            Genre = songDto.Genre,
            ReleaseDate = songDto.ReleaseDate,
            DurationSeconds = songDto.DurationSeconds,
            Album = albumRepository.GetById(songDto.AlbumId),
            Artists = songDto.ArtistsIds.Select(artistRepository.GetById).ToList(),
            SimilarSongs = songDto.SimilarSongsIds.Select(id => new SongLink { SongId = songDto.Id, SimilarSongId = id }).ToList(),
            CreatedAt = songDto.CreatedAt,
            UpdatedAt = songDto.UpdatedAt,
            DeletedAt = songDto.DeletedAt
        };
    }
}