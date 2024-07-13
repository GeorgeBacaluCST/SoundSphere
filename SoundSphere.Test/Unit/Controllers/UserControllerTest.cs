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
using static SoundSphere.Test.Mocks.RoleMock;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Unit.Controllers
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly UserController _userController;
        private readonly IMapper _mapper;

        private readonly UserDto _userDto1 = GetUserDto1();
        private readonly UserDto _userDto2 = GetUserDto2();
        private readonly List<UserDto> _userDtos = GetUserDtos();
        private readonly Role _role1 = GetRole1();
        private readonly Role _role2 = GetRole2();
        private readonly List<Authority> _authoritiesAdmin = GetAuthoritiesAdmin();
        private readonly List<Authority> _authoritiesModerator = GetAuthoritiesModerator();

        public UserControllerTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _userController = new(_userServiceMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _userServiceMock.Setup(mock => mock.GetAll()).Returns(_userDtos);
            OkObjectResult? result = _userController.GetAll() as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_userDtos);
        }

        [Fact] public void GetById_Test()
        {
            _userServiceMock.Setup(mock => mock.GetById(ValidUserGuid)).Returns(_userDto1);
            OkObjectResult? result = _userController.GetById(ValidUserGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(_userDto1);
        }

        [Fact] public void Add_Test()
        {
            User newUser = new()
            {
                Id = ValidPlaylistGuid,
                Name = "new_user_name",
                Email = "new_user_email@email.com",
                Password = "#New_user1_password!",
                Mobile = "+40700000002",
                Address = "new_user_address",
                Birthday = new DateOnly(2000, 1, 3),
                Avatar = "https://new-user-avatar.jpg",
                Role = _role1,
                Authorities = _authoritiesAdmin,
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0)
            };
            UserDto newUserDto = newUser.ToDto(_mapper);
            _userServiceMock.Setup(mock => mock.Add(It.IsAny<UserDto>())).Returns(newUserDto);
            CreatedAtActionResult? result = _userController.GetById(ValidUserGuid) as CreatedAtActionResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(newUserDto);
        }

        [Fact] public void UpdateById_Test()
        {
            User updatedUser = new()
            {
                Id = ValidUserGuid,
                Name = "updated_user_name",
                Email = "updated_user_email@email.com",
                Password = "#Updated_user1_password!",
                Mobile = "+40700000003",
                Address = "updated_user_address",
                Birthday = new DateOnly(2000, 1, 4),
                Avatar = "https://updated-user-avatar.jpg",
                Role = _role2,
                Authorities = _authoritiesModerator,
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
                UpdatedAt = new DateTime(2024, 7, 2, 0, 0, 0)
            };
            UserDto updatedUserDto = updatedUser.ToDto(_mapper);
            _userServiceMock.Setup(mock => mock.UpdateById(It.IsAny<UserDto>(), ValidUserGuid)).Returns(updatedUserDto);
            OkObjectResult? result = _userController.UpdateById(updatedUserDto, ValidUserGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(updatedUserDto);
        }

        [Fact] public void DeleteById_Test()
        {
            User deletedUser = new()
            {
                Id = ValidUserGuid,
                Name = "deleted_user_name",
                Email = "deleted_user_email@email.com",
                Password = "#Deleted_user1_password!",
                Mobile = "+40700000004",
                Address = "deleted_user_address",
                Birthday = new DateOnly(2000, 1, 4),
                Avatar = "https://deleted-user-avatar.jpg",
                Role = _role2,
                Authorities = _authoritiesModerator,
                CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
                UpdatedAt = new DateTime(2024, 7, 2, 0, 0, 0),
                DeletedAt = new DateTime(2024, 7, 3, 0, 0, 0)
            };
            UserDto deletedUserDto = deletedUser.ToDto(_mapper);
            _userServiceMock.Setup(mock => mock.DeleteById(ValidUserGuid)).Returns(deletedUserDto);
            OkObjectResult? result = _userController.DeleteById(ValidUserGuid) as OkObjectResult;
            result?.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            result?.Value.Should().BeEquivalentTo(deletedUserDto);
        }
    }
}