using Microsoft.EntityFrameworkCore;

using TheGalaxy.Database.Infrastructure;
using TheGalaxy.Database.Repositories.Base;
using TheGalaxy.Interfaces.Domain.Transport;

namespace TheGalaxy.Database.Repositories.Transports
{
    public class TransportRepository : BaseRepository, ITransportRepository
    {
        public TransportRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task<Guid> CreateTransport(UserTransport userTransport)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var existTransport = context.Transport.Where(x => x.Number == userTransport.Number);

                if (!existTransport.Any())
                {
                    context.Transport.Add(userTransport);
                    await context.SaveChangesAsync();

                    return userTransport.Id;
                }

                return existTransport.First().Id;
            }
        }

        public async Task<UserTransport?> GetById(Guid id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Transport.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<UserTransport?> GetByNumber(string number)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Transport.FirstOrDefaultAsync(x => x.Number == number);
            }
        }
    }
}
