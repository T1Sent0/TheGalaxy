using TheGalaxy.Interfaces.Domain.Roles;

namespace TheGalaxy.Interfaces.Database.Roles.Queries
{
    public interface IDbGetRoleByCodeQueryResult
    {
        public Role Role { get; set; }
    }
}
