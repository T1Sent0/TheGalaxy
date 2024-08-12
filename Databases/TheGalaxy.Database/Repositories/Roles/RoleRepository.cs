using MassTransit.Internals.GraphValidation;

using Microsoft.EntityFrameworkCore;

using TheGalaxy.Database.Infrastructure;
using TheGalaxy.Database.Repositories.Base;
using TheGalaxy.Interfaces.Domain.Roles;
using TheGalaxy.Interfaces.Enums.Enums;

namespace TheGalaxy.Database.Repositories.Roles
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task<Guid> CreateRole(Role role)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var existRole = context.Role.Where(x => x.Code == role.Code);
                    if (existRole.Any()) return existRole.First().Id;

                    context.Role.Add(role);
                    await context.SaveChangesAsync();

                    return role.Id;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Role?> GetRoleByode(RoleEnum code)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Role.FirstOrDefaultAsync(x => x.Code == code);
            }
        }
    }
}
