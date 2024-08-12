using TheGalaxy.Interfaces.Domain.Roles;

namespace TheGalaxy.Interfaces.Database.Roles.Commands
{
    public interface IDbCreateRoleCommand
    {
        public Role Role { get; set; }
    }
}
