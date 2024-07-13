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
using static SoundSphere.Test.Mocks.PlaylistMock;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Unit.Services
{
    public class PlaylistServiceTest
    {
        private readonly Mock<IPlaylistRepository> _playlistRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<ISongRepository> _songRepositoryMock = new();
        private readonly IPlaylistService _playlistService;
        private readonly IMapper _mapper;

        private readonly Playlist _playlist1 = GetPlaylist1();
        private readonly Playlist _playlist2 = GetPlaylist2();
        private readonly List<Playlist> _playlists = GetPlaylists();
        private readonly PlaylistDto _playlistDto1 = GetPlaylistDto1();
        private readonly PlaylistDto _playlistDto2 = GetPlaylistDto2();
        private readonly List<PlaylistDto> _playlistDtos = GetPlaylistDtos();
        private readonly User _user1 = GetUser1();

        public PlaylistServiceTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _playlistService = new PlaylistService(_playlistRepositoryMock.Object, _userRepositoryMock.Object, _songRepositoryMock.Object, _mapper);
        }

        [Fact] public void GetAll_Test()
        {
            _playlistRepositoryMock.Setup(mock => mock.GetAll()).Returns(_playlists);
            _playlistService.GetAll().Should().BeEquivalentTo(_playlistDtos);
        }

        [Fact] public void GetById_ValidId_Test()
        {
            _playlistRepositoryMock.Setup(mock => mock.GetById(ValidPlaylistGuid)).Returns(_playlist1);
            _playlistService.GetById(ValidPlaylistGuid).Should().BeEquivalentTo(_playlistDto1);
        }

        [Fact] public void GetById_InvalidId_Test()
        {
            _playlistRepositoryMock.Setup(mock => mock.GetById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(PlaylistNotFound, InvalidGuid)));
            _playlistService.Invoking(service => service.GetById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(PlaylistNotFound, InvalidGuid));
            _playlistRepositoryMock.Verify(mock => mock.GetById(InvalidGuid));
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
            _userRepositoryMock.Setup(mock => mock.GetById(ValidUserGuid)).Returns(_user1);
            _playlistRepositoryMock.Setup(mock => mock.Add(It.IsAny<Playlist>())).Returns(newPlaylist);
            _playlistService.Add(newPlaylistDto).Should().BeEquivalentTo(newPlaylistDto);
            _playlistRepositoryMock.Verify(mock => mock.Add(It.IsAny<Playlist>()));
        }

        [Fact] public void UpdateById_ValidId_Test()
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
            _playlistRepositoryMock.Setup(mock => mock.UpdateById(It.IsAny<Playlist>(), ValidPlaylistGuid)).Returns(updatedPlaylist);
            _playlistService.UpdateById(updatedPlaylistDto, ValidPlaylistGuid).Should().BeEquivalentTo(updatedPlaylistDto);
            _playlistRepositoryMock.Verify(mock => mock.UpdateById(It.IsAny<Playlist>(), ValidPlaylistGuid));
        }

        [Fact] public void UpdateById_InvalidId_Test()
        {
            PlaylistDto invalidPlaylistDto = new()
            {
                Id = InvalidGuid,
                Title = "",
                UserId = Guid.Empty,
                SongsIds = new List<Guid>(),
            };
            _playlistRepositoryMock.Setup(mock => mock.UpdateById(It.IsAny<Playlist>(), InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(PlaylistNotFound, InvalidGuid)));
            _playlistService.Invoking(service => service.UpdateById(invalidPlaylistDto, InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(PlaylistNotFound, InvalidGuid));
            _playlistRepositoryMock.Verify(mock => mock.UpdateById(It.IsAny<Playlist>(), InvalidGuid));
        }

        [Fact] public void DeleteById_ValidId_Test()
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
            _playlistRepositoryMock.Setup(mock => mock.DeleteById(ValidPlaylistGuid)).Returns(deletedPlaylist);
            _playlistService.DeleteById(ValidPlaylistGuid).Should().BeEquivalentTo(deletedPlaylistDto);
            _playlistRepositoryMock.Verify(mock => mock.DeleteById(ValidPlaylistGuid));
        }

        [Fact] public void DeleteById_InvalidId_Test()
        {
            _playlistRepositoryMock.Setup(mock => mock.DeleteById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(PlaylistNotFound, InvalidGuid)));
            _playlistService.Invoking(service => service.DeleteById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(PlaylistNotFound, InvalidGuid));
            _playlistRepositoryMock.Verify(mock => mock.DeleteById(InvalidGuid));
        }
    }
}