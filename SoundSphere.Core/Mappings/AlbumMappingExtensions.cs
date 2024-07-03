using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Mappings
{
    public static class AlbumMappingExtensions
    {
        public static IList<AlbumDto> ToDtos(this IList<Album> albums) => albums.Select(album => album.ToDto()).ToList();

        public static IList<Album> ToEntities(this IList<AlbumDto> albumDtos) => albumDtos.Select(albumDto => albumDto.ToEntity()).ToList();

        public static AlbumDto ToDto(this Album album) => new AlbumDto
        {
            Id = album.Id,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbumsIds = album.SimilarAlbums.Select(albumLink => albumLink.SimilarAlbumId).ToList(),
            CreatedAt = album.CreatedAt,
            UpdatedAt = album.UpdatedAt,
            DeletedAt = album.DeletedAt
        };

        public static Album ToEntity(this AlbumDto albumDto) => new Album
        {
            Id = albumDto.Id,
            Title = albumDto.Title,
            ImageUrl = albumDto.ImageUrl,
            ReleaseDate = albumDto.ReleaseDate,
            SimilarAlbums = albumDto.SimilarAlbumsIds.Select(id => new AlbumLink { AlbumId = albumDto.Id, SimilarAlbumId = id }).ToList(),
            CreatedAt = albumDto.CreatedAt,
            UpdatedAt = albumDto.UpdatedAt,
            DeletedAt = albumDto.DeletedAt
        };
    }
}