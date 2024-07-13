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
using static SoundSphere.Test.Mocks.ArtistMock;
using static SoundSphere.Test.Mocks.SongMock;

namespace SoundSphere.Test.Unit.Controllers
{
    public class SongControllerTest
    {
        private readonly Mock<ISongService> _songServiceMock = new();
        private readonly SongController _songController;
        private readonly IMapper _mapper;

        private readonly SongDto _songDto1 = GetSongDto1();
        private readonly SongDto _songDto2 = GetSongDto2();
        private readonly List<SongDto> _songDtos = GetSongDtos();
        private readonly Album _album1 = GetAlbum1();
        private readonly Album _album2 = GetAlbum2();
        private readonly List<Artist> _artists1 = [GetArtist1()];
        private readonly List<Artist> _artists2 = [GetArtist2()];

        public SongControllerTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _songController = new(_songServiceMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _songServiceMock.Setup(mock => mock.GetAll()).Returns(_songDtos);
            OkObjectResult? result = _songController.GetAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_songDtos);
        }

        [Fact] public void GetById_Test()
        {
            _songServiceMock.Setup(mock => mock.GetById(ValidSongGuid)).Returns(_songDto1);
            OkObjectResult? result = _songController.GetById(ValidSongGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_songDto1);
        }

        [Fact] public void Add_Test()
        {
            Song newSong = new()
            {
                Id = ValidSongGuid,
                Title = "new_song_title",
                ImageUrl = "https://new-song-imageurl.jpg",
                Genre = Genre.Pop,
                ReleaseDate = new DateOnly(2020, 1, 3),
                DurationSeconds = 190,
                Album = _album1,
                Artists = _artists1,
                SimilarSongs = [],
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0)
            };
            SongDto newSongDto = newSong.ToDto(_mapper);
            _songServiceMock.Setup(mock => mock.Add(It.IsAny<SongDto>())).Returns(newSongDto);
            CreatedAtActionResult? result = _songController.GetById(ValidSongGuid) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(newSongDto);
        }

        [Fact] public void UpdateById_Test()
        {
            Song updatedSong = new()
            {
                Id = ValidSongGuid,
                Title = "updated_song_title",
                ImageUrl = "https://updated-song-imageurl.jpg",
                Genre = Genre.Rock,
                ReleaseDate = new DateOnly(2020, 1, 4),
                DurationSeconds = 195,
                Album = _album2,
                Artists = _artists2,
                SimilarSongs = [],
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
                UpdatedAt = new DateTime(2024, 7, 2, 0, 0, 0)
            };
            SongDto updatedSongDto = updatedSong.ToDto(_mapper);
            _songServiceMock.Setup(mock => mock.UpdateById(It.IsAny<SongDto>(), ValidSongGuid)).Returns(updatedSongDto);
            OkObjectResult? result = _songController.UpdateById(updatedSongDto, ValidSongGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(updatedSongDto);
        }

        [Fact] public void DeleteById_Test()
        {
            Song deletedSong = new()
            {
                Id = ValidSongGuid,
                Title = "deleted_song_title",
                ImageUrl = "https://deleted-song-imageurl.jpg",
                Genre = Genre.Rnb,
                ReleaseDate = new DateOnly(2020, 1, 4),
                DurationSeconds = 200,
                Album = _album2,
                Artists = _artists2,
                SimilarSongs = [],
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
                UpdatedAt = new DateTime(2024, 7, 2, 0, 0, 0),
                DeletedAt = new DateTime(2024, 7, 3, 0, 0, 0)
            };
            SongDto deletedSongDto = deletedSong.ToDto(_mapper);
            _songServiceMock.Setup(mock => mock.DeleteById(ValidSongGuid)).Returns(deletedSongDto);
            OkObjectResult? result = _songController.DeleteById(ValidSongGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(deletedSongDto);
        }
    }
}