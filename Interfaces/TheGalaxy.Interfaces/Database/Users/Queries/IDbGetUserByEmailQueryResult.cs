using TheGalaxy.Interfaces.Domain.Users;

namespace TheGalaxy.Interfaces.Database.Users.Queries
{
    public interface IDbGetUserByEmailQueryResult
    {
        public User User { get; set; }
    }
}
