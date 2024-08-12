using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TheGalaxy.Database.Infrastructure;
using TheGalaxy.Database.Repositories.Roles;
using TheGalaxy.Database.Repositories.Transports;
using TheGalaxy.Database.Repositories.Users;

namespace TheGalaxy.Database.Configurate
{
    public static class ConfigurateService
    {
        public static void ConfigureService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration["DATABASE_CONNECTION"], config =>
                {
                    config.CommandTimeout(5);
                    config.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });
            });

            RegistrateRepositories(services);
        }

        private static void RegistrateRepositories(IServiceCollection services)
        {
            services.AddDbContextFactory<ApplicationDbContext>();

            services.AddHostedService<SeebDbHostedService>();
            services.AddScoped<SeedDb>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITransportRepository, TransportRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
