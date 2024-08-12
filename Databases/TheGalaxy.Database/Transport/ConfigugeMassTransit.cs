using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TheGalaxy.Database.Services;
using TheGalaxy.Interfaces.Database.Roles.Commands;
using TheGalaxy.Interfaces.Database.Roles.Queries;
using TheGalaxy.Interfaces.Database.Users.Commands;
using TheGalaxy.Interfaces.Database.Users.Queries;

namespace TheGalaxy.Database.Transport
{
    public static class ConfigugeMassTransit
    {
        public static void ConfigureBusControl(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<DbUserService>();
                x.AddConsumer<DbRoleService>();
                x.AddConsumer<DbTransportService>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);

                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });

                x.AddRequestClient<IDbCreateUserCommand>();
                x.AddRequestClient<IDbUpdateUserCommand>();
                x.AddRequestClient<IDbGetUserByEmailQuery>();
                x.AddRequestClient<IDbGetRoleByCodeQuery>();
                x.AddRequestClient<IDbCreateRoleCommand>();
            });

            services.AddMassTransitHostedService();
        }
    }
}
