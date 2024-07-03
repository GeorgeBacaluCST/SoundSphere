using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class AuthorityService : IAuthorityService
    {
        private readonly IAuthorityRepository _authorityRepository;

        public AuthorityService(IAuthorityRepository authorityRepository) => _authorityRepository = authorityRepository;

        public IList<AuthorityDto> GetAll() => _authorityRepository.GetAll().ToDtos();

        public AuthorityDto GetById(Guid id) => _authorityRepository.GetById(id).ToDto();

        public AuthorityDto Add(AuthorityDto authorityDto) => _authorityRepository.Add(authorityDto.ToEntity()).ToDto();
    }
}