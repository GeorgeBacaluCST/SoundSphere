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
using static SoundSphere.Test.Mocks.AlbumMock;

namespace SoundSphere.Test.Unit.Controllers
{
    public class AlbumControllerTest
    {
        private readonly Mock<IAlbumService> _albumServiceMock = new();
        private readonly AlbumController _albumController;
        private readonly IMapper _mapper;

        private readonly AlbumDto _albumDto1 = GetAlbumDto1();
        private readonly AlbumDto _albumDto2 = GetAlbumDto2();
        private readonly List<AlbumDto> _albumDtos = GetAlbumDtos();

        public AlbumControllerTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _albumController = new(_albumServiceMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _albumServiceMock.Setup(mock => mock.GetAll()).Returns(_albumDtos);
            OkObjectResult? result = _albumController.GetAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_albumDtos);
        }

        [Fact] public void GetById_Test()
        {
            _albumServiceMock.Setup(mock => mock.GetById(ValidAlbumGuid)).Returns(_albumDto1);
            OkObjectResult? result = _albumController.GetById(ValidAlbumGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_albumDto1);
        }

        [Fact] public void Add_Test()
        {
            Album newAlbum = new()
            {
                Id = ValidAlbumGuid,
                Title = "new_album_title",
                ImageUrl = "https://new-album-imageurl.jpg",
                ReleaseDate = new DateOnly(2020, 1, 3),
                SimilarAlbums = [],
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0)
            };
            AlbumDto newAlbumDto = newAlbum.ToDto(_mapper);
            _albumServiceMock.Setup(mock => mock.Add(It.IsAny<AlbumDto>())).Returns(newAlbumDto);
            CreatedAtActionResult? result = _albumController.GetById(ValidAlbumGuid) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(newAlbumDto);
        }

        [Fact] public void UpdateById_Test()
        {
            Album updatedAlbum = new()
            {
                Id = ValidAlbumGuid,
                Title = "updated_album_title",
                ImageUrl = "https://updated-album-imageurl.jpg",
                ReleaseDate = new DateOnly(2020, 1, 4),
                SimilarAlbums = [],
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
                UpdatedAt = new DateTime(2024, 7, 2, 0, 0, 0)
            };
            AlbumDto updatedAlbumDto = updatedAlbum.ToDto(_mapper);
            _albumServiceMock.Setup(mock => mock.UpdateById(It.IsAny<AlbumDto>(), ValidAlbumGuid)).Returns(updatedAlbumDto);
            OkObjectResult? result = _albumController.UpdateById(updatedAlbumDto, ValidAlbumGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(updatedAlbumDto);
        }

        [Fact] public void DeleteById_Test()
        {
            Album deletedAlbum = new()
            {
                Id = ValidAlbumGuid,
                Title = "deleted_album_title",
                ImageUrl = "https://deleted-album-imageurl.jpg",
                ReleaseDate = new DateOnly(2020, 1, 4),
                SimilarAlbums = [],
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
                UpdatedAt = new DateTime(2024, 7, 2, 0, 0, 0),
                DeletedAt = new DateTime(2024, 7, 3, 0, 0, 0)
            };
            AlbumDto deletedAlbumDto = deletedAlbum.ToDto(_mapper);
            _albumServiceMock.Setup(mock => mock.DeleteById(ValidAlbumGuid)).Returns(deletedAlbumDto);
            OkObjectResult? result = _albumController.DeleteById(ValidAlbumGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(deletedAlbumDto);
        }
    }
}