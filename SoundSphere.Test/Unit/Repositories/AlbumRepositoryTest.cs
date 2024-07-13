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

namespace SoundSphere.Test.Unit.Repositories
{
    public class AlbumRepositoryTest
    {
        private readonly Mock<DbSet<Album>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IAlbumRepository _albumRepository;

        private readonly Album _album1 = GetAlbum1();
        private readonly Album _album2 = GetAlbum2();
        private readonly List<Album> _albums = GetAlbums();

        public AlbumRepositoryTest()
        {
            IQueryable<Album> queryableAlbums = _albums.AsQueryable();
            _dbSetMock.As<IQueryable<Album>>().Setup(mock => mock.Provider).Returns(queryableAlbums.Provider);
            _dbSetMock.As<IQueryable<Album>>().Setup(mock => mock.Expression).Returns(queryableAlbums.Expression);
            _dbSetMock.As<IQueryable<Album>>().Setup(mock => mock.ElementType).Returns(queryableAlbums.ElementType);
            _dbSetMock.As<IQueryable<Album>>().Setup(mock => mock.GetEnumerator()).Returns(queryableAlbums.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Albums).Returns(_dbSetMock.Object);
            _albumRepository = new AlbumRepository(_dbContextMock.Object);
        }

        [Fact] public void GetAll_Test() => _albumRepository.GetAll().Should().BeEquivalentTo(_albums);

        [Fact] public void GetById_ValidId_Test() => _albumRepository.GetById(ValidAlbumGuid).Should().BeEquivalentTo(_album1);

        [Fact] public void GetById_InvalidId_Test() => _albumRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(AlbumNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            Album newAlbum = new() { Title = "new_album_title", ImageUrl = "https://new-album-imageurl.jpg", ReleaseDate = new DateOnly(2020, 1, 3), SimilarAlbums = [] };
            Album result = _albumRepository.Add(newAlbum);
            result.Should().BeEquivalentTo(newAlbum, options => options.Excluding(album => album.Id).Excluding(album => album.CreatedAt));
            result.CreatedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Album updatedAlbum = new() { Title = "updated_album_title", ImageUrl = "https://updated-album-imageurl.jpg", ReleaseDate = new DateOnly(2020, 1, 4), SimilarAlbums = [] };
            Mock<EntityEntry<Album>> entryMock = new();
            entryMock.SetupProperty(mock => mock.State, EntityState.Modified);
            _dbContextMock.SetupProperty(mock => mock.Entry(It.IsAny<Album>()), entryMock.Object);
            Album result = _albumRepository.UpdateById(updatedAlbum, ValidAlbumGuid);
            result.Should().BeEquivalentTo(updatedAlbum, options => options.Excluding(album => album.Id).Excluding(album => album.CreatedAt).Excluding(album => album.UpdatedAt));
            result.UpdatedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _albumRepository
            .Invoking(repository => repository.UpdateById(It.IsAny<Album>(), InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(AlbumNotFound, InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            Album result = _albumRepository.DeleteById(ValidAlbumGuid);
            result.DeletedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _albumRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(AlbumNotFound, InvalidGuid));
    }
}