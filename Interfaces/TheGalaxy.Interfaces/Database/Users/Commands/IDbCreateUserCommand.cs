using TheGalaxy.Interfaces.Domain.Users;

namespace TheGalaxy.Interfaces.Database.Users.Commands
{
    public interface IDbCreateUserCommand
    {
        public User NewUser { get; set; }
    }
}
