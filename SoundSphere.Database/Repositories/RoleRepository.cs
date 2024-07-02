﻿using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SoundSphereDbContext _context;

        public RoleRepository(SoundSphereDbContext context) => _context = context;

        public IList<Role> GetAll() => _context.Roles
            .OrderBy(role => role.CreatedAt)
            .ToList();

        public Role GetById(Guid id) => _context.Roles
            .SingleOrDefault(role => role.Id == id)
            ?? throw new Exception($"Role with id {id} not found");

        public Role Add(Role role)
        {
            if (role.Id == Guid.Empty) role.Id = Guid.NewGuid();
            role.CreatedAt = DateTime.Now;
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role;
        }
    }
}