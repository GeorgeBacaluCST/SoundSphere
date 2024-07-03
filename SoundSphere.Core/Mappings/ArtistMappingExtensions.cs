using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Mappings
{
    public static class ArtistMappingExtensions
    {
        public static IList<ArtistDto> ToDtos(this IList<Artist> artists) => artists.Select(artist => artist.ToDto()).ToList();

        public static IList<Artist> ToEntities(this IList<ArtistDto> artistDtos) => artistDtos.Select(artistDto => artistDto.ToEntity()).ToList();

        public static ArtistDto ToDto(this Artist artist) => new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Bio = artist.Bio,
            SimilarArtistsIds = artist.SimilarArtists.Select(artistLink => artistLink.SimilarArtistId).ToList(),
            CreatedAt = artist.CreatedAt,
            UpdatedAt = artist.UpdatedAt,
            DeletedAt = artist.DeletedAt
        };

        public static Artist ToEntity(this ArtistDto artistDto) => new Artist
        {
            Id = artistDto.Id,
            Name = artistDto.Name,
            ImageUrl = artistDto.ImageUrl,
            Bio = artistDto.Bio,
            SimilarArtists = artistDto.SimilarArtistsIds.Select(id => new ArtistLink { ArtistId = artistDto.Id, SimilarArtistId = id }).ToList(),
            CreatedAt = artistDto.CreatedAt,
            UpdatedAt = artistDto.UpdatedAt,
            DeletedAt = artistDto.DeletedAt
        };
    }
}