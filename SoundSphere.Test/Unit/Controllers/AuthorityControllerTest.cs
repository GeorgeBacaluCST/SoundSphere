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
using static SoundSphere.Test.Mocks.AuthorityMock;

namespace SoundSphere.Test.Unit.Controllers
{
    public class AuthorityControllerTest
    {
        private readonly Mock<IAuthorityService> _authorityServiceMock = new();
        private readonly AuthorityController _authorityController;
        private readonly IMapper _mapper;

        private readonly AuthorityDto _authorityDto1 = GetAuthorityDto1();
        private readonly AuthorityDto _authorityDto2 = GetAuthorityDto2();
        private readonly List<AuthorityDto> _authorityDtos = GetAuthorityDtosAdmin();

        public AuthorityControllerTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _authorityController = new(_authorityServiceMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _authorityServiceMock.Setup(mock => mock.GetAll()).Returns(_authorityDtos);
            OkObjectResult? result = _authorityController.GetAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_authorityDtos);
        }

        [Fact] public void GetById_Test()
        {
            _authorityServiceMock.Setup(mock => mock.GetById(ValidAuthorityGuid)).Returns(_authorityDto1);
            OkObjectResult? result = _authorityController.GetById(ValidAuthorityGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_authorityDto1);
        }

        [Fact] public void Add_Test()
        {
            Authority newAuthority = new() { Id = ValidAuthorityGuid, Type = AuthorityType.Create, CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0) };
            AuthorityDto newAuthorityDto = newAuthority.ToDto(_mapper);
            _authorityServiceMock.Setup(mock => mock.Add(It.IsAny<AuthorityDto>())).Returns(newAuthorityDto);
            CreatedAtActionResult? result = _authorityController.GetById(ValidAuthorityGuid) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(newAuthorityDto);
        }
    }
}