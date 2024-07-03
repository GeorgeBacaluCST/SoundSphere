using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IRoleService
    {
        IList<Role> GetAll();

        Role GetById(Guid id);

        Role Add(Role role);
    }
}