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
using static SoundSphere.Test.Mocks.ArtistMock;

namespace SoundSphere.Test.Unit.Repositories
{
    public class ArtistRepositoryTest
    {
        private readonly Mock<DbSet<Artist>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IArtistRepository _artistRepository;

        private readonly Artist _artist1 = GetArtist1();
        private readonly Artist _artist2 = GetArtist2();
        private readonly List<Artist> _artists = GetArtists();

        public ArtistRepositoryTest()
        {
            IQueryable<Artist> queryableArtists = _artists.AsQueryable();
            _dbSetMock.As<IQueryable<Artist>>().Setup(mock => mock.Provider).Returns(queryableArtists.Provider);
            _dbSetMock.As<IQueryable<Artist>>().Setup(mock => mock.Expression).Returns(queryableArtists.Expression);
            _dbSetMock.As<IQueryable<Artist>>().Setup(mock => mock.ElementType).Returns(queryableArtists.ElementType);
            _dbSetMock.As<IQueryable<Artist>>().Setup(mock => mock.GetEnumerator()).Returns(queryableArtists.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Artists).Returns(_dbSetMock.Object);
            _artistRepository = new ArtistRepository(_dbContextMock.Object);
        }

        [Fact] public void GetAll_Test() => _artistRepository.GetAll().Should().BeEquivalentTo(_artists);

        [Fact] public void GetById_ValidId_Test() => _artistRepository.GetById(ValidArtistGuid).Should().BeEquivalentTo(_artist1);

        [Fact] public void GetById_InvalidId_Test() => _artistRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(ArtistNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            Artist newArtist = new() { Name = "new_artist_name", ImageUrl = "https://new-artist-imageurl.jpg", Bio = "new_artist_bio", SimilarArtists = [] };
            Artist result = _artistRepository.Add(newArtist);
            result.Should().BeEquivalentTo(newArtist, options => options.Excluding(artist => artist.Id).Excluding(artist => artist.CreatedAt));
            result.CreatedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            Artist updatedArtist = new() { Name = "updated_artist_name", ImageUrl = "https://updated-artist-imageurl.jpg", Bio = "updated_artist_bio", SimilarArtists = [] };
            Mock<EntityEntry<Artist>> entryMock = new();
            entryMock.SetupProperty(mock => mock.State, EntityState.Modified);
            _dbContextMock.SetupProperty(mock => mock.Entry(It.IsAny<Artist>()), entryMock.Object);
            Artist result = _artistRepository.UpdateById(updatedArtist, ValidArtistGuid);
            result.Should().BeEquivalentTo(updatedArtist, options => options.Excluding(artist => artist.Id).Excluding(artist => artist.CreatedAt).Excluding(artist => artist.UpdatedAt));
            result.UpdatedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _artistRepository
            .Invoking(repository => repository.UpdateById(It.IsAny<Artist>(), InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(ArtistNotFound, InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            Artist result = _artistRepository.DeleteById(ValidArtistGuid);
            result.DeletedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _artistRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(ArtistNotFound, InvalidGuid));
    }
}