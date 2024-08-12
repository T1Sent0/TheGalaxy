using MassTransit;

using TheGalaxy.Interfaces.Core.Transports.Commands;
using TheGalaxy.Interfaces.Core.Transports.Queries;
using TheGalaxy.Interfaces.Database.Transports.Commands;
using TheGalaxy.Interfaces.Database.Transports.Queries;
using TheGalaxy.Interfaces.Domain.Transport;
using TheGalaxy.Interfaces.Models;

namespace TheGalaxy.Core.Services.Transports
{
    public class TransportService : IConsumer<IGetTransportByNumberQuery>,
        IConsumer<ICreatreTransportCommand>
    {
        private readonly IRequestClient<IDbGetTransportByNumberQuery> _dbGetTransportByNumberRequestClient;
        private readonly IRequestClient<IDbCreateTransportCommand> _dbCreateTransportCommandRequestClient;

        public TransportService(IRequestClient<IDbGetTransportByNumberQuery> dbGetTransportByNumberRequestClient,
            IRequestClient<IDbCreateTransportCommand> dbCreateTransportCommandRequestClient)
        {
            _dbGetTransportByNumberRequestClient = dbGetTransportByNumberRequestClient;
            _dbCreateTransportCommandRequestClient = dbCreateTransportCommandRequestClient;
        }

        public async Task Consume(ConsumeContext<IGetTransportByNumberQuery> context)
        {
            var message = context.Message;

            var entityTransport = await _dbGetTransportByNumberRequestClient.GetResponse<IDbGetTransportByNumberQueryResult>(new { message.Number });

            var transportDto = new UserTransportDto();
            transportDto.UserId = entityTransport.Message.Transport.UserId;
            transportDto.Number = entityTransport.Message.Transport.Number;

            await context.RespondAsync<IGetTransportByNumberQueryResult>(new { UserTransport = transportDto });
        }

        public async Task Consume(ConsumeContext<ICreatreTransportCommand> context)
        {
            var message = context.Message;

            var domainTransport = new UserTransport();
            domainTransport.UserId = message.UserTransport.UserId;
            domainTransport.Number = message.UserTransport.Number;

            var createResult = await _dbCreateTransportCommandRequestClient.GetResponse<IDbCreateTransportCommandResult>(new { UserTransport = domainTransport });

            await context.RespondAsync<ICreatreTransportCommandResult>(new { createResult.Message.Id });
        }
    }
}
