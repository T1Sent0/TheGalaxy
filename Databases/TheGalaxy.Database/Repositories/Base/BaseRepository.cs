using Microsoft.EntityFrameworkCore;

using TheGalaxy.Database.Infrastructure;

namespace TheGalaxy.Database.Repositories.Base
{
    public class BaseRepository
    {
        public readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public BaseRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
    }
}
