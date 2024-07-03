using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IAuthorityService
    {
        IList<Authority> GetAll();

        Authority GetById(Guid id);

        Authority Add(Authority authority);
    }
}