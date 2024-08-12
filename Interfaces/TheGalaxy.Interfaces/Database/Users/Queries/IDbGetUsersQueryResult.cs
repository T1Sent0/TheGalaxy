using TheGalaxy.Interfaces.Domain.Users;

namespace TheGalaxy.Interfaces.Database.Users.Queries
{
    public interface IDbGetUsersQueryResult
    {
        public User[] Users {  get; set; }
    }
}
