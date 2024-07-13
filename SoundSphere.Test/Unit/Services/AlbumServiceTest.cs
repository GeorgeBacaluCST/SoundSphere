using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Test.Mocks.AlbumMock;

namespace SoundSphere.Test.Unit.Services
{
    public class AlbumServiceTest
    {
        private readonly Mock<IAlbumRepository> _albumRepositoryMock = new();
        private readonly IAlbumService _albumService;
        private readonly IMapper _mapper;

        private readonly Album _album1 = GetAlbum1();
        private readonly Album _album2 = GetAlbum2();
        private readonly List<Album> _albums = GetAlbums();
        private readonly AlbumDto _albumDto1 = GetAlbumDto1();
        private readonly AlbumDto _albumDto2 = GetAlbumDto2();
        private readonly List<AlbumDto> _albumDtos = GetAlbumDtos();

        public AlbumServiceTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _albumService = new AlbumService(_albumRepositoryMock.Object, _mapper);
        }

        [Fact] public void GetAll_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.GetAll()).Returns(_albums);
            _albumService.GetAll().Should().BeEquivalentTo(_albumDtos);
        }

        [Fact] public void GetById_ValidId_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.GetById(ValidAlbumGuid)).Returns(_album1);
            _albumService.GetById(ValidAlbumGuid).Should().BeEquivalentTo(_albumDto1);
        }

        [Fact] public void GetById_InvalidId_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.GetById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(AlbumNotFound, InvalidGuid)));
            _albumService.Invoking(service => service.GetById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(AlbumNotFound, InvalidGuid));
            _albumRepositoryMock.Verify(mock => mock.GetById(InvalidGuid));
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
            _albumRepositoryMock.Setup(mock => mock.Add(It.IsAny<Album>())).Returns(newAlbum);
            _albumService.Add(newAlbumDto).Should().BeEquivalentTo(newAlbumDto);
            _albumRepositoryMock.Verify(mock => mock.Add(It.IsAny<Album>()));
        }

        [Fact] public void UpdateById_ValidId_Test()
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
            _albumRepositoryMock.Setup(mock => mock.UpdateById(It.IsAny<Album>(), ValidAlbumGuid)).Returns(updatedAlbum);
            _albumService.UpdateById(updatedAlbumDto, ValidAlbumGuid).Should().BeEquivalentTo(updatedAlbumDto);
            _albumRepositoryMock.Verify(mock => mock.UpdateById(It.IsAny<Album>(), ValidAlbumGuid));
        }

        [Fact] public void UpdateById_InvalidId_Test()
        {
            AlbumDto invalidAlbumDto = new()
            {
                Id = InvalidGuid,
                Title = "",
                ImageUrl = "",
                ReleaseDate = new DateOnly(2020, 1, 5),
                SimilarAlbumsIds = new List<Guid>()
            };
            _albumRepositoryMock.Setup(mock => mock.UpdateById(It.IsAny<Album>(), InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(AlbumNotFound, InvalidGuid)));
            _albumService.Invoking(service => service.UpdateById(invalidAlbumDto, InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(AlbumNotFound, InvalidGuid));
            _albumRepositoryMock.Verify(mock => mock.UpdateById(It.IsAny<Album>(), InvalidGuid));
        }

        [Fact] public void DeleteById_ValidId_Test()
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
            _albumRepositoryMock.Setup(mock => mock.DeleteById(ValidAlbumGuid)).Returns(deletedAlbum);
            _albumService.DeleteById(ValidAlbumGuid).Should().BeEquivalentTo(deletedAlbumDto);
            _albumRepositoryMock.Verify(mock => mock.DeleteById(ValidAlbumGuid));
        }

        [Fact] public void DeleteById_InvalidId_Test()
        {
            _albumRepositoryMock.Setup(mock => mock.DeleteById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(AlbumNotFound, InvalidGuid)));
            _albumService.Invoking(service => service.DeleteById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(AlbumNotFound, InvalidGuid));
            _albumRepositoryMock.Verify(mock => mock.DeleteById(InvalidGuid));
        }
    }
}