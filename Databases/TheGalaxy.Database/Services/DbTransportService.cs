using MassTransit;

using TheGalaxy.Database.Repositories.Transports;
using TheGalaxy.Interfaces.Database.Transports.Commands;
using TheGalaxy.Interfaces.Database.Transports.Queries;

namespace TheGalaxy.Database.Services
{
    public class DbTransportService : IConsumer<IDbCreateTransportCommand>,
        IConsumer<IDbGetTransportByNumberQuery>, IConsumer<IDbGetTransportByIdQuery>
    {
        private readonly ITransportRepository _transportRepository;

        public DbTransportService(ITransportRepository transportRepository) 
        {
            _transportRepository = transportRepository;
        }

        public async Task Consume(ConsumeContext<IDbCreateTransportCommand> context)
        {
            var message = context.Message;
            var createResult = await _transportRepository.CreateTransport(message.UserTransport);

            await context.RespondAsync<IDbCreateTransportCommandResult>(new { Id = createResult });
        }

        public async Task Consume(ConsumeContext<IDbGetTransportByNumberQuery> context)
        {
            var message = context.Message;
            var userTransport = await _transportRepository.GetByNumber(message.Number);

            await context.RespondAsync<IDbGetTransportByNumberQueryResult>(new { Transport = userTransport });
        }

        public async Task Consume(ConsumeContext<IDbGetTransportByIdQuery> context)
        {
            var message = context.Message;

            var userTransport = await _transportRepository.GetById(message.Id);

            await context.RespondAsync<IDbGetTransportByIdQueryResult>(new { Transport = userTransport });
        }
    }
}
