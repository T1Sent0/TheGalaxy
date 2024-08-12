using TheGalaxy.Interfaces.Models;

namespace TheGalaxy.Interfaces.Core.Users.Queries
{
    public interface IGetUsersQueryResult
    {
        public UserDto[] Users { get; set; }
    }
}
