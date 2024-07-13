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
using static SoundSphere.Test.Mocks.AlbumMock;
using static SoundSphere.Test.Mocks.ArtistMock;
using static SoundSphere.Test.Mocks.SongMock;

namespace SoundSphere.Test.Unit.Services
{
    public class SongServiceTest
    {
        private readonly Mock<ISongRepository> _songRepositoryMock = new();
        private readonly Mock<IAlbumRepository> _albumRepositoryMock = new();
        private readonly Mock<IArtistRepository> _artistRepositoryMock = new();
        private readonly ISongService _songService;
        private readonly IMapper _mapper;

        private readonly Song _song1 = GetSong1();
        private readonly Song _song2 = GetSong2();
        private readonly List<Song> _songs = GetSongs();
        private readonly SongDto _songDto1 = GetSongDto1();
        private readonly SongDto _songDto2 = GetSongDto2();
        private readonly List<SongDto> _songDtos = GetSongDtos();
        private readonly Album _album1 = GetAlbum1();
        private readonly Album _album2 = GetAlbum2();
        private readonly List<Artist> _artists1 = [GetArtist1()];
        private readonly List<Artist> _artists2 = [GetArtist2()];

        public SongServiceTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _songService = new SongService(_songRepositoryMock.Object, _albumRepositoryMock.Object, _artistRepositoryMock.Object, _mapper);
        }

        [Fact] public void GetAll_Test()
        {
            _songRepositoryMock.Setup(mock => mock.GetAll()).Returns(_songs);
            _songService.GetAll().Should().BeEquivalentTo(_songDtos);
        }

        [Fact] public void GetById_ValidId_Test()
        {
            _songRepositoryMock.Setup(mock => mock.GetById(ValidSongGuid)).Returns(_song1);
            _songService.GetById(ValidSongGuid).Should().BeEquivalentTo(_songDto1);
        }

        [Fact] public void GetById_InvalidId_Test()
        {
            _songRepositoryMock.Setup(mock => mock.GetById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(SongNotFound, InvalidGuid)));
            _songService.Invoking(service => service.GetById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(SongNotFound, InvalidGuid));
            _songRepositoryMock.Verify(mock => mock.GetById(InvalidGuid));
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
            _songRepositoryMock.Setup(mock => mock.Add(It.IsAny<Song>())).Returns(newSong);
            _songService.Add(newSongDto).Should().BeEquivalentTo(newSongDto);
            _songRepositoryMock.Verify(mock => mock.Add(It.IsAny<Song>()));
        }

        [Fact] public void UpdateById_ValidId_Test()
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
            _songRepositoryMock.Setup(mock => mock.UpdateById(It.IsAny<Song>(), ValidSongGuid)).Returns(updatedSong);
            _songService.UpdateById(updatedSongDto, ValidSongGuid).Should().BeEquivalentTo(updatedSongDto);
            _songRepositoryMock.Verify(mock => mock.UpdateById(It.IsAny<Song>(), ValidSongGuid));
        }

        [Fact] public void UpdateById_InvalidId_Test()
        {
            SongDto invalidSongDto = new()
            {
                Id = InvalidGuid,
                Title = "",
                ImageUrl = "",
                Genre = Genre.InvalidGenre,
                ReleaseDate = new DateOnly(2020, 1, 5),
                DurationSeconds = 0,
                AlbumId = Guid.Empty,
                ArtistsIds = new List<Guid>(),
                SimilarSongsIds = new List<Guid>()
            };
            _songRepositoryMock.Setup(mock => mock.UpdateById(It.IsAny<Song>(), InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(SongNotFound, InvalidGuid)));
            _songService.Invoking(service => service.UpdateById(invalidSongDto, InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(SongNotFound, InvalidGuid));
            _songRepositoryMock.Verify(mock => mock.UpdateById(It.IsAny<Song>(), InvalidGuid));
        }

        [Fact] public void DeleteById_ValidId_Test()
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
            _songRepositoryMock.Setup(mock => mock.DeleteById(ValidSongGuid)).Returns(deletedSong);
            _songService.DeleteById(ValidSongGuid).Should().BeEquivalentTo(deletedSongDto);
            _songRepositoryMock.Verify(mock => mock.DeleteById(ValidSongGuid));
        }

        [Fact] public void DeleteById_InvalidId_Test()
        {
            _songRepositoryMock.Setup(mock => mock.DeleteById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(SongNotFound, InvalidGuid)));
            _songService.Invoking(service => service.DeleteById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(SongNotFound, InvalidGuid));
            _songRepositoryMock.Verify(mock => mock.DeleteById(InvalidGuid));
        }
    }
}