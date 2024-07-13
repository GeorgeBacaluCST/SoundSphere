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
using static SoundSphere.Test.Mocks.AuthorityMock;

namespace SoundSphere.Test.Unit.Services
{
    public class AuthorityServiceTest
    {
        private readonly Mock<IAuthorityRepository> _authorityRepositoryMock = new();
        private readonly IAuthorityService _authorityService;
        private readonly IMapper _mapper;

        private readonly Authority _authority1 = GetAuthority1();
        private readonly Authority _authority2 = GetAuthority2();
        private readonly List<Authority> _authorities = GetAuthoritiesAdmin();
        private readonly AuthorityDto _authorityDto1 = GetAuthorityDto1();
        private readonly AuthorityDto _authorityDto2 = GetAuthorityDto2();
        private readonly List<AuthorityDto> _authorityDtos = GetAuthorityDtosAdmin();

        public AuthorityServiceTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _authorityService = new AuthorityService(_authorityRepositoryMock.Object, _mapper);
        }

        [Fact] public void GetAll_Test()
        {
            _authorityRepositoryMock.Setup(mock => mock.GetAll()).Returns(_authorities);
            _authorityService.GetAll().Should().BeEquivalentTo(_authorityDtos);
        }

        [Fact]
        public void GetById_ValidId_Test()
        {
            _authorityRepositoryMock.Setup(mock => mock.GetById(ValidAuthorityGuid)).Returns(_authority1);
            _authorityService.GetById(ValidAuthorityGuid).Should().BeEquivalentTo(_authorityDto1);
        }

        [Fact]
        public void GetById_InvalidId_Test()
        {
            _authorityRepositoryMock.Setup(mock => mock.GetById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(AuthorityNotFound, InvalidGuid)));
            _authorityService.Invoking(service => service.GetById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(AuthorityNotFound, InvalidGuid));
            _authorityRepositoryMock.Verify(mock => mock.GetById(InvalidGuid));
        }

        [Fact]
        public void Add_Test()
        {
            Authority newAuthority = new() { Id = ValidAuthorityGuid, Type = AuthorityType.Create, CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0) };
            AuthorityDto newAuthorityDto = newAuthority.ToDto(_mapper);
            _authorityRepositoryMock.Setup(mock => mock.Add(It.IsAny<Authority>())).Returns(newAuthority);
            _authorityService.Add(newAuthorityDto).Should().BeEquivalentTo(newAuthorityDto);
            _authorityRepositoryMock.Verify(mock => mock.Add(It.IsAny<Authority>()));
        }
    }
}