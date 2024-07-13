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
using static SoundSphere.Test.Mocks.AlbumMock;
using static SoundSphere.Test.Mocks.ArtistMock;
using static SoundSphere.Test.Mocks.SongMock;

namespace SoundSphere.Test.Unit.Repositories
{
    public class SongRepositoryTest
    {
        private readonly Mock<DbSet<Song>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly ISongRepository _songRepository;

        private readonly Song _song1 = GetSong1();
        private readonly Song _song2 = GetSong2();
        private readonly List<Song> _songs = GetSongs();
        private readonly Album _album1 = GetAlbum1();
        private readonly Album _album2 = GetAlbum2();
        private readonly List<Artist> _artists1 = [GetArtist1()];
        private readonly List<Artist> _artists2 = [GetArtist2()];

        public SongRepositoryTest()
        {
            IQueryable<Song> queryableSongs = _songs.AsQueryable();
            _dbSetMock.As<IQueryable<Song>>().Setup(mock => mock.Provider).Returns(queryableSongs.Provider);
            _dbSetMock.As<IQueryable<Song>>().Setup(mock => mock.Expression).Returns(queryableSongs.Expression);
            _dbSetMock.As<IQueryable<Song>>().Setup(mock => mock.ElementType).Returns(queryableSongs.ElementType);
            _dbSetMock.As<IQueryable<Song>>().Setup(mock => mock.GetEnumerator()).Returns(queryableSongs.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Songs).Returns(_dbSetMock.Object);
            _songRepository = new SongRepository(_dbContextMock.Object);
        }

        [Fact] public void GetAll_Test() => _songRepository.GetAll().Should().BeEquivalentTo(_songs);

        [Fact] public void GetById_ValidId_Test() => _songRepository.GetById(ValidSongGuid).Should().BeEquivalentTo(_song1);

        [Fact] public void GetById_InvalidId_Test() => _songRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(SongNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            Song newSong = new() 
            { 
                Title = "new_song_title",
                ImageUrl = "https://new-song-imageurl.jpg",
                Genre = Genre.Pop,
                ReleaseDate = new DateOnly(2020, 1, 3),
                DurationSeconds = 190,
                Album = _album1,
                Artists = _artists1,
                SimilarSongs = []
            };
            Song result = _songRepository.Add(newSong);
            result.Should().BeEquivalentTo(newSong, options => options.Excluding(song => song.Id).Excluding(song => song.CreatedAt));
            result.CreatedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Song updatedSong = new()
            {
                Title = "updated_song_title",
                ImageUrl = "https://updated-song-imageurl.jpg",
                Genre = Genre.Rock,
                ReleaseDate = new DateOnly(2020, 1, 4),
                DurationSeconds = 195,
                Album = _album2,
                Artists = _artists2,
                SimilarSongs = []
            };
            Mock<EntityEntry<Song>> entryMock = new();
            entryMock.SetupProperty(mock => mock.State, EntityState.Modified);
            _dbContextMock.SetupProperty(mock => mock.Entry(It.IsAny<Song>()), entryMock.Object);
            Song result = _songRepository.UpdateById(updatedSong, ValidSongGuid);
            result.Should().BeEquivalentTo(updatedSong, options => options.Excluding(song => song.Id).Excluding(song => song.CreatedAt).Excluding(song => song.UpdatedAt));
            result.UpdatedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _songRepository
            .Invoking(repository => repository.UpdateById(It.IsAny<Song>(), InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(SongNotFound, InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            Song result = _songRepository.DeleteById(ValidSongGuid);
            result.DeletedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _songRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(SongNotFound, InvalidGuid));
    }
}