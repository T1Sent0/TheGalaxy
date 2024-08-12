using MassTransit;

using Microsoft.Extensions.DependencyInjection;

using TheGalaxy.Core.Services.Auth;
using TheGalaxy.Core.Services.Transports;
using TheGalaxy.Core.Services.Users;
using TheGalaxy.Interfaces.Database.Transports.Commands;
using TheGalaxy.Interfaces.Database.Transports.Queries;
using TheGalaxy.Interfaces.Database.Users.Commands;
using TheGalaxy.Interfaces.Database.Users.Queries;

namespace TheGalaxy.Core.Transport
{
    public static class ConfigugeMassTransit
    {
        public static void RegisterService(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<AuthService>();
                x.AddConsumer<UserService>();
                x.AddConsumer<TransportService>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);

                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });

                x.AddRequestClient<IDbGetUserByIdQuery>();
                x.AddRequestClient<IDbUpdateUserCommand>();
                x.AddRequestClient<IDbGetUsersQuery>();
                x.AddRequestClient<IDbGetUserByEmailQuery>();

                x.AddRequestClient<IDbGetTransportByNumberQuery>();
                x.AddRequestClient<IDbCreateTransportCommand>();

            });

            services.AddMassTransitHostedService();
        }
    }
}
