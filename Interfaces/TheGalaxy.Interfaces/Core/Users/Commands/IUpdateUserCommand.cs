using TheGalaxy.Interfaces.Models;

namespace TheGalaxy.Interfaces.Core.Users.Commands
{
    public interface IUpdateUserCommand
    {
        public UserDto User { get; set; }
    }
}
