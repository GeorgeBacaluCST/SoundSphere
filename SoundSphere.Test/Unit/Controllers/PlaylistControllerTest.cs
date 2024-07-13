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
using static SoundSphere.Test.Mocks.PlaylistMock;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Unit.Controllers
{
    public class PlaylistControllerTest
    {
        private readonly Mock<IPlaylistService> _playlistServiceMock = new();
        private readonly PlaylistController _playlistController;
        private readonly IMapper _mapper;

        private readonly PlaylistDto _playlistDto1 = GetPlaylistDto1();
        private readonly PlaylistDto _playlistDto2 = GetPlaylistDto2();
        private readonly List<PlaylistDto> _playlistDtos = GetPlaylistDtos();
        private readonly User _user1 = GetUser1();

        public PlaylistControllerTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _playlistController = new(_playlistServiceMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _playlistServiceMock.Setup(mock => mock.GetAll()).Returns(_playlistDtos);
            OkObjectResult? result = _playlistController.GetAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_playlistDtos);
        }

        [Fact] public void GetById_Test()
        {
            _playlistServiceMock.Setup(mock => mock.GetById(ValidPlaylistGuid)).Returns(_playlistDto1);
            OkObjectResult? result = _playlistController.GetById(ValidPlaylistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_playlistDto1);
        }

        [Fact] public void Add_Test()
        {
            Playlist newPlaylist = new()
            {
                Id = ValidPlaylistGuid,
                Title = "new_playlist_title",
                User = _user1,
                Songs = [],
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0)
            };
            PlaylistDto newPlaylistDto = newPlaylist.ToDto(_mapper);
            _playlistServiceMock.Setup(mock => mock.Add(It.IsAny<PlaylistDto>())).Returns(newPlaylistDto);
            CreatedAtActionResult? result = _playlistController.GetById(ValidPlaylistGuid) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(newPlaylistDto);
        }

        [Fact] public void UpdateById_Test()
        {
            Playlist updatedPlaylist = new()
            {
                Id = ValidPlaylistGuid,
                Title = "update_playlist_title",
                User = _user1,
                Songs = [],
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
                UpdatedAt = new DateTime(2024, 7, 2, 0, 0, 0)
            };
            PlaylistDto updatedPlaylistDto = updatedPlaylist.ToDto(_mapper);
            _playlistServiceMock.Setup(mock => mock.UpdateById(It.IsAny<PlaylistDto>(), ValidPlaylistGuid)).Returns(updatedPlaylistDto);
            OkObjectResult? result = _playlistController.UpdateById(updatedPlaylistDto, ValidPlaylistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(updatedPlaylistDto);
        }

        [Fact] public void DeleteById_Test()
        {
            Playlist deletedPlaylist = new()
            {
                Id = ValidPlaylistGuid,
                Title = "deleted_playlist_title",
                User = _user1,
                Songs = [],
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
                UpdatedAt = new DateTime(2024, 7, 2, 0, 0, 0),
                DeletedAt = new DateTime(2024, 7, 3, 0, 0, 0)
            };
            PlaylistDto deletedPlaylistDto = deletedPlaylist.ToDto(_mapper);
            _playlistServiceMock.Setup(mock => mock.DeleteById(ValidPlaylistGuid)).Returns(deletedPlaylistDto);
            OkObjectResult? result = _playlistController.DeleteById(ValidPlaylistGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(deletedPlaylistDto);
        }
    }
}