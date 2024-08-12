using TheGalaxy.Interfaces.Enums.Enums;

namespace TheGalaxy.Interfaces.Database.Roles.Queries
{
    public interface IDbGetRoleByCodeQuery
    {
        public RoleEnum Code { get; set; }
    }
}
