using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Database.Repositories;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Test.Mocks.AuthorityMock;
using static SoundSphere.Test.Mocks.RoleMock;
using static SoundSphere.Test.Mocks.UserMock;

namespace SoundSphere.Test.Unit.Repositories
{
    public class UserRepositoryTest
    {
        private readonly Mock<DbSet<User>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IUserRepository _userRepository;

        private readonly User _user1 = GetUser1();
        private readonly User _user2 = GetUser2();
        private readonly List<User> _users = GetUsers();
        private readonly Role _role1 = GetRole1();
        private readonly Role _role2 = GetRole2();
        private readonly List<Authority> _authoritiesAdmin = GetAuthoritiesAdmin();
        private readonly List<Authority> _authoritiesModerator = GetAuthoritiesModerator();

        public UserRepositoryTest()
        {
            IQueryable<User> queryableUsers = _users.AsQueryable();
            _dbSetMock.As<IQueryable<User>>().Setup(mock => mock.Provider).Returns(queryableUsers.Provider);
            _dbSetMock.As<IQueryable<User>>().Setup(mock => mock.Expression).Returns(queryableUsers.Expression);
            _dbSetMock.As<IQueryable<User>>().Setup(mock => mock.ElementType).Returns(queryableUsers.ElementType);
            _dbSetMock.As<IQueryable<User>>().Setup(mock => mock.GetEnumerator()).Returns(queryableUsers.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Users).Returns(_dbSetMock.Object);
            _userRepository = new UserRepository(_dbContextMock.Object);
        }

        [Fact] public void GetAll_Test() => _userRepository.GetAll().Should().BeEquivalentTo(_users);

        [Fact] public void GetById_ValidId_Test() => _userRepository.GetById(ValidUserGuid).Should().BeEquivalentTo(_user1);

        [Fact] public void GetById_InvalidId_Test() => _userRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(UserNotFound, InvalidGuid));

        [Fact] public void Add_Test()
        {
            User newUser = new()
            {
                Name = "new_user_name",
                Email = "new_user_email@email.com",
                Password = "#New_user_password",
                Mobile = "+40700000002",
                Address = "new_user_address",
                Birthday = new DateOnly(2000, 1, 3),
                Avatar = "https://new-user-avatar.jpg",
                Role = _role1,
                Authorities = _authoritiesAdmin
            };
            User result = _userRepository.Add(newUser);
            result.Should().BeEquivalentTo(newUser, options => options.Excluding(user => user.Id).Excluding(user => user.CreatedAt));
            result.CreatedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_ValidId_Test()
        {
            User updatedUser = new()
            {
                Name = "updated_user_name",
                Email = "updated_user_email@email.com",
                Password = "#Updated_user_password",
                Mobile = "+40700000003",
                Address = "updated_user_address",
                Birthday = new DateOnly(2000, 1, 4),
                Avatar = "https://update-user-avatar.jpg",
                Role = _role2,
                Authorities = _authoritiesModerator
            };
            Mock<EntityEntry<User>> entryMock = new();
            entryMock.SetupProperty(mock => mock.State, EntityState.Modified);
            _dbContextMock.SetupProperty(mock => mock.Entry(It.IsAny<User>()), entryMock.Object);
            User result = _userRepository.UpdateById(updatedUser, ValidUserGuid);
            result.Should().BeEquivalentTo(updatedUser, options => options.Excluding(user => user.Id).Excluding(user => user.CreatedAt).Excluding(user => user.UpdatedAt));
            result.UpdatedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void UpdateById_InvalidId_Test() => _userRepository
            .Invoking(repository => repository.UpdateById(It.IsAny<User>(), InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(UserNotFound, InvalidGuid));

        [Fact] public void DeleteById_ValidId_Test()
        {
            User result = _userRepository.DeleteById(ValidUserGuid);
            result.DeletedAt.Should().NotBeNull();
            _dbContextMock.Verify(mock => mock.SaveChanges());
        }

        [Fact] public void DeleteById_InvalidId_Test() => _userRepository
            .Invoking(repository => repository.DeleteById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(UserNotFound, InvalidGuid));
    }
}