using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository) => _roleRepository = roleRepository;

        public IList<Role> GetAll() => _roleRepository.GetAll();

        public Role GetById(Guid id) => _roleRepository.GetById(id);

        public Role Add(Role role) => _roleRepository.Add(role);
    }
}