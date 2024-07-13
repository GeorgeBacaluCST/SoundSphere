using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Test.Mocks
{
    public class ArtistMock
    {
        private ArtistMock() { }

        public static List<Artist> GetArtists() => [GetArtist1(), GetArtist2()];

        public static List<ArtistDto> GetArtistDtos() => GetArtists().Select(ToDto).ToList();

        public static Artist GetArtist1() => new()
        {
            Id = Guid.Parse("4e75ecdd-aafe-4c35-836b-1b83fc7b8f88"),
            Name = "artist_name1",
            ImageUrl = "https://artist-imageurl1.jpg",
            Bio = "artist_bio1",
            SimilarArtists = [new() { ArtistId = Guid.Parse("4e75ecdd-aafe-4c35-836b-1b83fc7b8f88"), SimilarArtistId = Guid.Parse("8c301aa9-6d56-4c06-b1f2-9b9956979345") }],
            CreatedAt = new DateTime(2024, 6, 1, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static Artist GetArtist2() => new()
        {
            Id = Guid.Parse("8c301aa9-6d56-4c06-b1f2-9b9956979345"),
            Name = "artist_name2",
            ImageUrl = "https://artist-imageurl2.jpg",
            Bio = "artist_bio2",
            SimilarArtists = [],
            CreatedAt = new DateTime(2024, 6, 2, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static ArtistDto GetArtistDto1() => ToDto(GetArtist1());

        public static ArtistDto GetArtistDto2() => ToDto(GetArtist2());

        public static ArtistDto ToDto(Artist artist) => new()
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
    }
}