using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoundSphere.Api.Controllers;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using static SoundSphere.Database.Constants;
using static SoundSphere.Test.Mocks.ArtistMock;

namespace SoundSphere.Test.Unit.Controllers
{
    public class ArtistControllerTest
    {
        private readonly Mock<IArtistService> _artistServiceMock = new();
        private readonly ArtistController _artistController;
        private readonly IMapper _mapper;

        private readonly ArtistDto _artistDto1 = GetArtistDto1();
        private readonly ArtistDto _artistDto2 = GetArtistDto2();
        private readonly List<ArtistDto> _artistDtos = GetArtistDtos();

        public ArtistControllerTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _artistController = new(_artistServiceMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _artistServiceMock.Setup(mock => mock.GetAll()).Returns(_artistDtos);
            OkObjectResult? result = _artistController.GetAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_artistDtos);
        }

        [Fact] public void GetById_Test()
        {
            _artistServiceMock.Setup(mock => mock.GetById(ValidArtistGuid)).Returns(_artistDto1);
            OkObjectResult? result = _artistController.GetById(ValidArtistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_artistDto1);
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
            _artistServiceMock.Setup(mock => mock.Add(It.IsAny<ArtistDto>())).Returns(newArtistDto);
            CreatedAtActionResult? result = _artistController.GetById(ValidArtistGuid) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(newArtistDto);
        }

        [Fact] public void UpdateById_Test()
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
            _artistServiceMock.Setup(mock => mock.UpdateById(It.IsAny<ArtistDto>(), ValidArtistGuid)).Returns(updatedArtistDto);
            OkObjectResult? result = _artistController.UpdateById(updatedArtistDto, ValidArtistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(updatedArtistDto);
        }

        [Fact] public void DeleteById_Test()
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
            _artistServiceMock.Setup(mock => mock.DeleteById(ValidArtistGuid)).Returns(deletedArtistDto);
            OkObjectResult? result = _artistController.DeleteById(ValidArtistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(deletedArtistDto);
        }
    }
}