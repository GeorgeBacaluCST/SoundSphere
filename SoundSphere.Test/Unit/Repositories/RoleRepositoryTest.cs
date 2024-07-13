using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Test.Mocks.RoleMock;

namespace SoundSphere.Test.Unit.Repositories
{
    public class RoleRepositoryTest
    {
        private readonly Mock<DbSet<Role>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IRoleRepository _roleRepository;

        private readonly Role _role1 = GetRole1();
        private readonly List<Role> _roles = GetRoles();

        public RoleRepositoryTest()
        {
            IQueryable<Role> queryableAuthorities = _roles.AsQueryable();
            _dbSetMock.As<IQueryable<Role>>().Setup(mock => mock.Provider).Returns(queryableAuthorities.Provider);
            _dbSetMock.As<IQueryable<Role>>().Setup(mock => mock.Expression).Returns(queryableAuthorities.Expression);
            _dbSetMock.As<IQueryable<Role>>().Setup(mock => mock.ElementType).Returns(queryableAuthorities.ElementType);
            _dbSetMock.As<IQueryable<Role>>().Setup(mock => mock.GetEnumerator()).Returns(queryableAuthorities.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Roles).Returns(_dbSetMock.Object);
            _roleRepository = new RoleRepository(_dbContextMock.Object);
        }

        [Fact] public void GetAll_Test() => _roleRepository.GetAll().Should().BeEquivalentTo(_roles);

        [Fact] public void GetById_ValidId_Test() => _roleRepository.GetById(ValidRoleGuid).Should().BeEquivalentTo(_role1);

        [Fact]
        public void GetById_InvalidId_Test() => _roleRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(RoleNotFound, InvalidGuid));

        [Fact]
        public void Add_Test() => _roleRepository
            .Invoking(repository => repository.Add(new() { Type = RoleType.Admin }))
            .Should().Throw<DbUpdateException>();
    }
}