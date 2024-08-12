using TheGalaxy.Interfaces.Domain.Users;

namespace TheGalaxy.Interfaces.Database.Users.Queries
{
    public interface IDbGetUserByIdQueryResult
    {
        public User User { get; set; }
    }
}
