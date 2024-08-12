using TheGalaxy.Interfaces.Domain.Roles;
using TheGalaxy.Interfaces.Enums.Enums;

namespace TheGalaxy.Database.Repositories.Roles
{
    public interface IRoleRepository
    {
        Task<Guid> CreateRole(Role role);
        Task<Role?> GetRoleByode(RoleEnum code);
    }
}
