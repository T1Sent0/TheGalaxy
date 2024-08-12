using TheGalaxy.Interfaces.Domain.Users;

namespace TheGalaxy.Database.Repositories.Users
{
    public interface IUserRepository
    {
        Task<bool> CreateUser(User user);

        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(Guid userId);
        Task<User[]> GetUsers();
        Task<bool> UpdateUser(User user);
    }
}
