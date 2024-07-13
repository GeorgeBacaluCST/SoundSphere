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
using static SoundSphere.Test.Mocks.RoleMock;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Unit.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IRoleRepository> _roleRepositoryMock = new();
        private readonly Mock<IAuthorityRepository> _authorityRepositoryMock = new();
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        private readonly User _user1 = GetUser1();
        private readonly User _user2 = GetUser2();
        private readonly List<User> _users = GetUsers();
        private readonly UserDto _userDto1 = GetUserDto1();
        private readonly UserDto _userDto2 = GetUserDto2();
        private readonly List<UserDto> _userDtos = GetUserDtos();
        private readonly Role _role1 = GetRole1();
        private readonly Role _role2 = GetRole2();
        private readonly List<Authority> _authoritiesAdmin = GetAuthoritiesAdmin();
        private readonly List<Authority> _authoritiesModerator = GetAuthoritiesModerator();

        public UserServiceTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>()).CreateMapper();
            _userService = new UserService(_userRepositoryMock.Object, _roleRepositoryMock.Object, _authorityRepositoryMock.Object, _mapper);
        }

        [Fact] public void GetAll_Test()
        {
            _userRepositoryMock.Setup(mock => mock.GetAll()).Returns(_users);
            _userService.GetAll().Should().BeEquivalentTo(_userDtos);
        }

        [Fact] public void GetById_ValidId_Test()
        {
            _userRepositoryMock.Setup(mock => mock.GetById(ValidUserGuid)).Returns(_user1);
            _userService.GetById(ValidUserGuid).Should().BeEquivalentTo(_userDto1);
        }

        [Fact] public void GetById_InvalidId_Test()
        {
            _userRepositoryMock.Setup(mock => mock.GetById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(UserNotFound, InvalidGuid)));
            _userService.Invoking(service => service.GetById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(UserNotFound, InvalidGuid));
            _userRepositoryMock.Verify(mock => mock.GetById(InvalidGuid));
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
            _userRepositoryMock.Setup(mock => mock.Add(It.IsAny<User>())).Returns(newUser);
            _userService.Add(newUserDto).Should().BeEquivalentTo(newUserDto);
            _userRepositoryMock.Verify(mock => mock.Add(It.IsAny<User>()));
        }

        [Fact] public void UpdateById_ValidId_Test()
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
            _userRepositoryMock.Setup(mock => mock.UpdateById(It.IsAny<User>(), ValidUserGuid)).Returns(updatedUser);
            _userService.UpdateById(updatedUserDto, ValidUserGuid).Should().BeEquivalentTo(updatedUserDto);
            _userRepositoryMock.Verify(mock => mock.UpdateById(It.IsAny<User>(), ValidUserGuid));
        }

        [Fact] public void UpdateById_InvalidId_Test()
        {
            UserDto invalidUserDto = new()
            {
                Id = InvalidGuid,
                Name = "",
                Email = "",
                Mobile = "",
                Address = "",
                Birthday = new DateOnly(2000, 1, 5),
                Avatar = "",
                RoleId = Guid.Empty,
                AuthoritiesIds = new List<Guid>()
            };
            _userRepositoryMock.Setup(mock => mock.UpdateById(It.IsAny<User>(), InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(UserNotFound, InvalidGuid)));
            _userService.Invoking(service => service.UpdateById(invalidUserDto, InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(UserNotFound, InvalidGuid));
            _userRepositoryMock.Verify(mock => mock.UpdateById(It.IsAny<User>(), InvalidGuid));
        }

        [Fact] public void DeleteById_ValidId_Test()
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
            _userRepositoryMock.Setup(mock => mock.DeleteById(ValidUserGuid)).Returns(deletedUser);
            _userService.DeleteById(ValidUserGuid).Should().BeEquivalentTo(deletedUserDto);
            _userRepositoryMock.Verify(mock => mock.DeleteById(ValidUserGuid));
        }

        [Fact] public void DeleteById_InvalidId_Test()
        {
            _userRepositoryMock.Setup(mock => mock.DeleteById(InvalidGuid)).Throws(new ResourceNotFoundException(string.Format(UserNotFound, InvalidGuid)));
            _userService.Invoking(service => service.DeleteById(InvalidGuid))
                .Should().Throw<ResourceNotFoundException>()
                .WithMessage(string.Format(UserNotFound, InvalidGuid));
            _userRepositoryMock.Verify(mock => mock.DeleteById(InvalidGuid));
        }
    }
}