using TheGalaxy.Interfaces.Models;

namespace TheGalaxy.Interfaces.Core.Users.Queries
{
    public interface IGetUserByIdQueryResult
    {
        public UserDto User { get; set; }
    }
}
