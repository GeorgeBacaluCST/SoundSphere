using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Test.Mocks.PlaylistMock;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Unit.Repositories
{
    public class PlaylistRepositoryTest
    {
        private readonly Mock<DbSet<Playlist>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IPlaylistRepository _playlistRepository;

        private readonly Playlist _playlist1 = GetPlaylist1();
        private readonly Playlist _playlist2 = GetPlaylist2();
        private readonly List<Playlist> _playlists = GetPlaylists();
        private readonly User _user1 = GetUser1();

        public PlaylistRepositoryTest()
        {
            IQueryable<Playlist> queryablePlaylists = _playlists.AsQueryable();
            _dbSetMock.As<IQueryable<Playlist>>().Setup(mock => mock.Provider).Returns(queryablePlaylists.Provider);
            _dbSetMock.As<IQueryable<Playlist>>().Setup(mock => mock.Expression).Returns(queryablePlaylists.Expression);
            _dbSetMock.As<IQueryable<Playlist>>().Setup(mock => mock.ElementType).Returns(queryablePlaylists.ElementType);
            _dbSetMock.As<IQueryable<Playlist>>().Setup(mock => mock.GetEnumerator()).Returns(queryablePlaylists.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Playlists).Returns(_dbSetMock.Object);
            _playlistRepository = new PlaylistRepository(_dbContextMock.Object);
        }

        [Fact] public void GetAll_Test() => _playlistRepository.GetAll().Should().BeEquivalentTo(_playlists);

        [Fact] public void GetById_ValidId_Test() => _playlistRepository.GetById(ValidPlaylistGuid).Should().BeEquivalentTo(_playlist1);

        [Fact] public void GetById_InvalidId_Test() => _playlistRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(PlaylistNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            Playlist newPlaylist = new() { Title = "new_playlist_title", User = _user1, Songs = [] };
            Playlist result = _playlistRepository.Add(newPlaylist);
            result.Should().BeEquivalentTo(newPlaylist, options => options.Excluding(playlist => playlist.Id).Excluding(playlist => playlist.CreatedAt));
            result.CreatedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Playlist updatedPlaylist = new() { Title = "update_playlist_title", User = _user1, Songs = [] };
            Mock<EntityEntry<Playlist>> entryMock = new();
            entryMock.SetupProperty(mock => mock.State, EntityState.Modified);
            _dbContextMock.SetupProperty(mock => mock.Entry(It.IsAny<Playlist>()), entryMock.Object);
            Playlist result = _playlistRepository.UpdateById(updatedPlaylist, ValidPlaylistGuid);
            result.Should().BeEquivalentTo(updatedPlaylist, options => options.Excluding(playlist => playlist.Id).Excluding(playlist => playlist.CreatedAt).Excluding(playlist => playlist.UpdatedAt));
            result.UpdatedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _playlistRepository
            .Invoking(repository => repository.UpdateById(It.IsAny<Playlist>(), InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(PlaylistNotFound, InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            Playlist result = _playlistRepository.DeleteById(ValidPlaylistGuid);
            result.DeletedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _playlistRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(PlaylistNotFound, InvalidGuid));
    }
}