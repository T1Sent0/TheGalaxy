using MassTransit;

using TheGalaxy.Interfaces.Core.Auth.Command;
using TheGalaxy.Interfaces.Core.Users.Queries;

namespace TheGalaxyApp
{
    public class ConfigureMassTransit
    {
        public static void AddMassTransitWithTransport(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });

                x.AddRequestClient<IGetUserTokenQuery>();
                x.AddRequestClient<IGetUsersQuery>();
            });

            services.AddMassTransitHostedService();
        }
    }
}
