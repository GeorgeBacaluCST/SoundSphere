using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class AuthorityService : IAuthorityService
    {
        private readonly IAuthorityRepository _authorityRepository;

        public AuthorityService(IAuthorityRepository authorityRepository) => _authorityRepository = authorityRepository;

        public IList<Authority> GetAll() => _authorityRepository.GetAll();

        public Authority GetById(Guid id) => _authorityRepository.GetById(id);

        public Authority Add(Authority authority) => _authorityRepository.Add(authority);
    }
}