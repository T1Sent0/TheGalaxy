using Microsoft.EntityFrameworkCore;

using TheGalaxy.Database.Infrastructure;
using TheGalaxy.Database.Repositories.Base;
using TheGalaxy.Interfaces.Domain.Users;

namespace TheGalaxy.Database.Repositories.Users
{
    internal class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task<bool> CreateUser(User user)
        {
            using(var context = _contextFactory.CreateDbContext())
            {
                var findUser = context.Users.Where(x => x.Email == user.Email);
            
                if (!findUser.Any())
                {
                    context.Users.Add(user);
                    await context.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email);
            }
        }

        public async Task<User?> GetUserById(Guid userId)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            }
        }

        public async Task<User[]> GetUsers()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Users.ToArrayAsync();
            }
        }

        public async Task<bool> UpdateUser(User user)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var existUser = context.Users.Where(x => x.Id == user.Id);
            
                if (existUser.Any())
                {
                    var userEntity = existUser.First();
                    userEntity.FirstName = user.FirstName;
                    userEntity.LastName = user.LastName;
                    userEntity.MiddleName = user.MiddleName;
                    userEntity.TranportId = user.TranportId;

                    await context.SaveChangesAsync();

                    return true;
                }

                return false;
            }
        }
    }
}
