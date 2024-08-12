using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using TheGalaxy.Interfaces.Domain.Roles;
using TheGalaxy.Interfaces.Domain.Transport;
using TheGalaxy.Interfaces.Domain.Users;

namespace TheGalaxy.Database.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        protected ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Role {  get; set; }

        public DbSet<UserTransport> Transport {  get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(GetConnectionString(_configuration));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.HasOne(x => x.Role)
                    .WithMany()
                    .HasForeignKey(x => x.RoleId)
                    .IsRequired();

                b.HasOne(x => x.Transport).WithOne(x => x.User);

                b.HasIndex(x => x.Email);
            });

            modelBuilder.Entity<UserTransport>(b =>
            {
                b.HasOne(x => x.User).WithOne(x => x.Transport);
                b.HasIndex(x => x.Number);

            });

            base.OnModelCreating(modelBuilder);
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=postgres";

            if (configuration != null)
            {
                connectionString = configuration["DATABASE_CONNECTION"];
            }

            return connectionString;
        }

        public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
        {
            private readonly IConfiguration _configuration;

            public DbContextFactory()
            {

            }

            public DbContextFactory(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public ApplicationDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseNpgsql(GetConnectionString(_configuration));
                var context = new ApplicationDbContext(optionsBuilder.Options, _configuration);

                return context;
            }
        }
    }
}
