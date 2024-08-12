using TheGalaxy.Interfaces.Domain.Users;

namespace TheGalaxy.Interfaces.Database.Users.Commands
{
    public interface IDbUpdateUserCommand
    {
        public User User { get; set; }
    }
}
