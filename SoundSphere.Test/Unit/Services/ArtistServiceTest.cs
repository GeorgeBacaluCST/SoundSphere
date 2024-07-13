using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Core.Services;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Test.Mocks.ArtistMock;

namespace SoundSphere.Test.Unit.Services
{
    public class ArtistServiceTest
    {
        private readonly Mock<IArtistRepository> _artistRepositoryMock = new();
        private readonly IArtistService _artistService;
        private readonly IMapper _mapper;

        private readonly Artist _artist1 = GetArtist1();
        private readonly Artist _artist2 = GetArtist2();
        private readonly List<Artist> _artists = GetArtists();
        private readonly ArtistDto _artistDto1 = GetArtistDto1();
        private readonly ArtistDto _artistDto2 = GetArtistDto2();
        private readonly List<ArtistDto> _artistDtos = GetArtistDtos();

        public ArtistServiceTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _artistService = new ArtistService(_artistRepositoryMock.Object, _mapper);
        }

        [Fact] public void GetAll_Test()
        {
            _artistRepositoryMock.Setup(mock => mock.GetAll()).Returns(_artists);
            _artistService.GetAll().Should().BeEquivalentTo(_artistDtos);
        }

        [Fact] public void GetById_ValidId_Test()
        {
            _artistRepositoryMock.Setup(mock => mock.GetById(ValidArtistGuid)).Returns(_artist1);
            _artistService.GetById(ValidArtistGuid).Should().BeEquivalentTo(_artistDto1);
        }

        [Fact] public void GetById_InvalidId_Test()
        {
            _artistRepositoryMock.Setup(mock => mock.GetById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(ArtistNotFound, InvalidGuid)));
            _artistService.Invoking(service => service.GetById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(ArtistNotFound, InvalidGuid));
            _artistRepositoryMock.Verify(mock => mock.GetById(InvalidGuid));
        }

        [Fact] public void Add_Test()
        {
            Artist newArtist = new()
            {
                Id = ValidArtistGuid,
                Name = "new_artist_name",
                ImageUrl = "https://new-artist-imageurl.jpg",
                Bio = "new_artist_bio",
                SimilarArtists = [],
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0)
            };
            ArtistDto newArtistDto = newArtist.ToDto(_mapper);
            _artistRepositoryMock.Setup(mock => mock.Add(It.IsAny<Artist>())).Returns(newArtist);
            _artistService.Add(newArtistDto).Should().BeEquivalentTo(newArtistDto);
            _artistRepositoryMock.Verify(mock => mock.Add(It.IsAny<Artist>()));
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Artist updatedArtist = new()
            {
                Id = ValidArtistGuid,
                Name = "updated_artist_name",
                ImageUrl = "https://updated-artist-imageurl.jpg",
                Bio = "updated_artist_bio",
                SimilarArtists = [],
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
                UpdatedAt = new DateTime(2024, 7, 2, 0, 0, 0)
            };
            ArtistDto updatedArtistDto = updatedArtist.ToDto(_mapper);
            _artistRepositoryMock.Setup(mock => mock.UpdateById(It.IsAny<Artist>(), ValidArtistGuid)).Returns(updatedArtist);
            _artistService.UpdateById(updatedArtistDto, ValidArtistGuid).Should().BeEquivalentTo(updatedArtistDto);
            _artistRepositoryMock.Verify(mock => mock.UpdateById(It.IsAny<Artist>(), ValidArtistGuid));
        }

        [Fact] public void UpdateById_InvalidId_Test()
        {
            ArtistDto invalidArtistDto = new()
            {
                Id = InvalidGuid,
                Name = "",
                ImageUrl = "",
                Bio = "",
                SimilarArtistsIds = new List<Guid>()
            };
            _artistRepositoryMock.Setup(mock => mock.UpdateById(It.IsAny<Artist>(), InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(ArtistNotFound, InvalidGuid)));
            _artistService.Invoking(service => service.UpdateById(invalidArtistDto, InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(ArtistNotFound, InvalidGuid));
            _artistRepositoryMock.Verify(mock => mock.UpdateById(It.IsAny<Artist>(), InvalidGuid));
        }

        [Fact] public void DeleteById_ValidId_Test()
        {
            Artist deletedArtist = new()
            {
                Id = ValidArtistGuid,
                Name = "deleted_artist_name",
                ImageUrl = "https://deleted-artist-imageurl.jpg",
                Bio = "",
                SimilarArtists = [],
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
                UpdatedAt = new DateTime(2024, 7, 2, 0, 0, 0),
                DeletedAt = new DateTime(2024, 7, 3, 0, 0, 0)
            };
            ArtistDto deletedArtistDto = deletedArtist.ToDto(_mapper);
            _artistRepositoryMock.Setup(mock => mock.DeleteById(ValidArtistGuid)).Returns(deletedArtist);
            _artistService.DeleteById(ValidArtistGuid).Should().BeEquivalentTo(deletedArtistDto);
            _artistRepositoryMock.Verify(mock => mock.DeleteById(ValidArtistGuid));
        }

        [Fact] public void DeleteById_InvalidId_Test()
        {
            _artistRepositoryMock.Setup(mock => mock.DeleteById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(ArtistNotFound, InvalidGuid)));
            _artistService.Invoking(service => service.DeleteById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(ArtistNotFound, InvalidGuid));
            _artistRepositoryMock.Verify(mock => mock.DeleteById(InvalidGuid));
        }
    }
}