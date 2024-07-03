using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository) => _roleRepository = roleRepository;

        public IList<RoleDto> GetAll() => _roleRepository.GetAll().ToDtos();

        public RoleDto GetById(Guid id) => _roleRepository.GetById(id).ToDto();

        public RoleDto Add(RoleDto roleDto) => _roleRepository.Add(roleDto.ToEntity()).ToDto();
    }
}