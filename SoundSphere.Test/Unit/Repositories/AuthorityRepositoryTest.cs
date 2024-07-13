using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;
using static SoundSphere.Test.Mocks.AuthorityMock;

namespace SoundSphere.Test.Unit.Repositories
{
    public class AuthorityRepositoryTest
    {
        private readonly Mock<DbSet<Authority>> _dbSetMock = new();
        private readonly Mock<SoundSphereDbContext> _dbContextMock = new();
        private readonly IAuthorityRepository _authorityRepository;

        private readonly Authority _authority1 = GetAuthority1();
        private readonly List<Authority> _authorities = GetAuthoritiesAdmin();

        public AuthorityRepositoryTest()
        {
            IQueryable<Authority> queryableAuthorities = _authorities.AsQueryable();
            _dbSetMock.As<IQueryable<Authority>>().Setup(mock => mock.Provider).Returns(queryableAuthorities.Provider);
            _dbSetMock.As<IQueryable<Authority>>().Setup(mock => mock.Expression).Returns(queryableAuthorities.Expression);
            _dbSetMock.As<IQueryable<Authority>>().Setup(mock => mock.ElementType).Returns(queryableAuthorities.ElementType);
            _dbSetMock.As<IQueryable<Authority>>().Setup(mock => mock.GetEnumerator()).Returns(queryableAuthorities.GetEnumerator());
            _dbContextMock.Setup(mock => mock.Authorities).Returns(_dbSetMock.Object);
            _authorityRepository = new AuthorityRepository(_dbContextMock.Object);
        }

        [Fact] public void GetAll_Test() => _authorityRepository.GetAll().Should().BeEquivalentTo(_authorities);

        [Fact] public void GetById_ValidId_Test() => _authorityRepository.GetById(ValidAuthorityGuid).Should().BeEquivalentTo(_authority1);

        [Fact] public void GetById_InvalidId_Test() => _authorityRepository
            .Invoking(repository => repository.GetById(InvalidGuid))
            .Should().Throw<ResourceNotFoundException>()
            .WithMessage(string.Format(AuthorityNotFound, InvalidGuid));

        [Fact] public void Add_Test() => _authorityRepository
            .Invoking(repository => repository.Add(new() { Type = AuthorityType.Create }))
            .Should().Throw<DbUpdateException>();
    }
}