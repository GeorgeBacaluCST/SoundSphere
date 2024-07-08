﻿using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;

namespace SoundSphere.Database.Repositories
{
    public class AuthorityRepository : IAuthorityRepository
    {
        private readonly SoundSphereDbContext _context;

        public AuthorityRepository(SoundSphereDbContext context) => _context = context;

        public IList<Authority> GetAll() => _context.Authorities
            .OrderBy(authority => authority.CreatedAt)
            .ToList();

        public Authority GetById(Guid id) => _context.Authorities
            .SingleOrDefault(authority => authority.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(AuthorityNotFound, id));

        public Authority Add(Authority authority)
        {
            if (authority.Id == Guid.Empty) authority.Id = Guid.NewGuid();
            authority.CreatedAt = DateTime.Now;
            _context.Authorities.Add(authority);
            _context.SaveChanges();
            return authority;
        }
    }
}