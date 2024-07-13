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
using static SoundSphere.Test.Mocks.RoleMock;

namespace SoundSphere.Test.Unit.Controllers
{
    public class RoleControllerTest
    {
        private readonly Mock<IRoleService> _authorityServiceMock = new();
        private readonly RoleController _authorityController;
        private readonly IMapper _mapper;

        private readonly RoleDto _authorityDto1 = GetRoleDto1();
        private readonly RoleDto _authorityDto2 = GetRoleDto2();
        private readonly List<RoleDto> _authorityDtos = GetRoleDtos();

        public RoleControllerTest()
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
            _authorityServiceMock.Setup(mock => mock.GetById(ValidRoleGuid)).Returns(_authorityDto1);
            OkObjectResult? result = _authorityController.GetById(ValidRoleGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_authorityDto1);
        }

        [Fact] public void Add_Test()
        {
            Role newRole = new() { Id = ValidRoleGuid, Type = RoleType.Admin, CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0) };
            RoleDto newRoleDto = newRole.ToDto(_mapper);
            _authorityServiceMock.Setup(mock => mock.Add(It.IsAny<RoleDto>())).Returns(newRoleDto);
            CreatedAtActionResult? result = _authorityController.GetById(ValidRoleGuid) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(newRoleDto);
        }
    }
}