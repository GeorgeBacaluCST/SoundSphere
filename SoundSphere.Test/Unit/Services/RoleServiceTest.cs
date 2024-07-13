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
using static SoundSphere.Test.Mocks.RoleMock;

namespace SoundSphere.Test.Unit.Services
{
    public class RoleServiceTest
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock = new();
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        private readonly Role _role1 = GetRole1();
        private readonly Role _role2 = GetRole2();
        private readonly List<Role> _roles = GetRoles();
        private readonly RoleDto _roleDto1 = GetRoleDto1();
        private readonly RoleDto _roleDto2 = GetRoleDto2();
        private readonly List<RoleDto> _roleDtos = GetRoleDtos();

        public RoleServiceTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _roleService = new RoleService(_roleRepositoryMock.Object, _mapper);
        }

        [Fact] public void GetAll_Test()
        {
            _roleRepositoryMock.Setup(mock => mock.GetAll()).Returns(_roles);
            _roleService.GetAll().Should().BeEquivalentTo(_roleDtos);
        }

        [Fact] public void GetById_ValidId_Test()
        {
            _roleRepositoryMock.Setup(mock => mock.GetById(ValidRoleGuid)).Returns(_role1);
            _roleService.GetById(ValidRoleGuid).Should().BeEquivalentTo(_roleDto1);
        }

        [Fact] public void GetById_InvalidId_Test()
        {
            _roleRepositoryMock.Setup(mock => mock.GetById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(RoleNotFound, InvalidGuid)));
            _roleService.Invoking(service => service.GetById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(RoleNotFound, InvalidGuid));
            _roleRepositoryMock.Verify(mock => mock.GetById(InvalidGuid));
        }

        [Fact] public void Add_Test()
        {
            Role newRole = new() { Id = ValidRoleGuid, Type = RoleType.Admin, CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0) };
            RoleDto newRoleDto = newRole.ToDto(_mapper);
            _roleRepositoryMock.Setup(mock => mock.Add(It.IsAny<Role>())).Returns(newRole);
            _roleService.Add(newRoleDto).Should().BeEquivalentTo(newRoleDto);
            _roleRepositoryMock.Verify(mock => mock.Add(It.IsAny<Role>()));
        }
    }
}