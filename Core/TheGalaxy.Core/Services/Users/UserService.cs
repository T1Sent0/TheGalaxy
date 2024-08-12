using MassTransit;

using TheGalaxy.Interfaces.Core.Transports.Queries;
using TheGalaxy.Interfaces.Core.Users.Commands;
using TheGalaxy.Interfaces.Core.Users.Queries;
using TheGalaxy.Interfaces.Database.Transports.Commands;
using TheGalaxy.Interfaces.Database.Transports.Queries;
using TheGalaxy.Interfaces.Database.Users.Commands;
using TheGalaxy.Interfaces.Database.Users.Queries;
using TheGalaxy.Interfaces.Domain.Transport;
using TheGalaxy.Interfaces.Domain.Users;
using TheGalaxy.Interfaces.Models;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TheGalaxy.Core.Services.Users
{
    public class UserService : IConsumer<IGetUserByIdQuery>, IConsumer<IUpdateUserCommand>,
        IConsumer<IGetUsersQuery>
    {
        private readonly IRequestClient<IDbGetUserByIdQuery> _dbGetUserByIdRequestClient;
        private readonly IRequestClient<IDbUpdateUserCommand> _dbUpdateUserRequestClient;
        private readonly IRequestClient<IDbGetUsersQuery> _dbGetUsersQueryRequestClient;
        private readonly IRequestClient<IDbGetTransportByNumberQuery> _dbGetTransportByNumberQueryClient;
        private readonly IRequestClient<IDbCreateTransportCommand> _dbCreateTransportCommandClient;
        private readonly IRequestClient<IDbGetTransportByIdQuery> _dbGetTransportByIdQueryClient;

        public UserService(IRequestClient<IDbGetUserByIdQuery> dbGetUserByIdRequestClient,
            IRequestClient<IDbUpdateUserCommand> dbUpdateUserRequestClient,
            IRequestClient<IDbGetUsersQuery> dbGetUsersQueryRequestClient,
            IRequestClient<IDbGetTransportByNumberQuery> dbGetTransportByNumberQueryClient,
            IRequestClient<IDbCreateTransportCommand> dbCreateTransportCommandClient,
            IRequestClient<IDbGetTransportByIdQuery> dbGetTransportByIdQueryClient)
        {
            _dbGetUserByIdRequestClient = dbGetUserByIdRequestClient;
            _dbUpdateUserRequestClient = dbUpdateUserRequestClient;
            _dbGetUsersQueryRequestClient = dbGetUsersQueryRequestClient;
            _dbGetTransportByNumberQueryClient = dbGetTransportByNumberQueryClient;
            _dbCreateTransportCommandClient = dbCreateTransportCommandClient;
            _dbGetTransportByIdQueryClient = dbGetTransportByIdQueryClient;
        }

        public async Task Consume(ConsumeContext<IGetUserByIdQuery> context)
        {
            var message = context.Message;

            var domainUser = await _dbGetUserByIdRequestClient.GetResponse<IDbGetUserByIdQueryResult>(new { message.Id });
            var userTransport = await _dbGetTransportByIdQueryClient.GetResponse<IDbGetTransportByIdQueryResult>(new { Id = domainUser.Message.User.TranportId });

            UserDto? userDto = null;
            if (domainUser.Message.User != null)
            {
                userDto = new UserDto();
                userDto.FirstName = domainUser.Message.User.FirstName;
                userDto.LastName = domainUser.Message.User.LastName;
                userDto.MiddleName = domainUser.Message.User.MiddleName;
                userDto.Role = domainUser.Message.User.Role.Code;
                userDto.CarNumber = userTransport.Message.Transport != null ? userTransport.Message.Transport.Number : string.Empty;
            }

            await context.RespondAsync<IGetUserByIdQueryResult>(new { User = userDto });
        }

        public async Task Consume(ConsumeContext<IUpdateUserCommand> context)
        {
            var message = context.Message;

            var transport = await _dbGetTransportByNumberQueryClient.GetResponse<IDbGetTransportByNumberQueryResult>(new { Number = message.User.CarNumber });
            Guid? newTransportId = null;

            if (transport.Message.Transport == null)
            {
                var newTransport = new UserTransport()
                {
                    Number = message.User.CarNumber,
                    UserId = message.User.Id
                };

                var createTransportResult = await _dbCreateTransportCommandClient.GetResponse<IDbCreateTransportCommandResult>(new { UserTransport = newTransport });
                newTransportId = createTransportResult.Message.Id;
            }

            var domainUser = new User();
            domainUser.Id = message.User.Id;
            domainUser.FirstName = message.User.FirstName;
            domainUser.LastName = message.User.LastName;
            domainUser.MiddleName = message.User.MiddleName;
            domainUser.TranportId = newTransportId ?? transport.Message.Transport.Id;

            var updateResult = await _dbUpdateUserRequestClient.GetResponse<IDbUpdateUserCommandResult>(new { User = domainUser });

            await context.RespondAsync<IUpdateUserCommandResult>(new { updateResult.Message.Success });
        }

        public async Task Consume(ConsumeContext<IGetUsersQuery> context)
        {
            var message = context.Message;

            var users = await _dbGetUsersQueryRequestClient.GetResponse<IDbGetUsersQueryResult>(new {});
            List<UserDto> result = new List<UserDto>();
            foreach (var item in users.Message.Users)
            {
                var userTransport = await _dbGetTransportByIdQueryClient.GetResponse<IDbGetTransportByIdQueryResult>(new { Id = item.TranportId });
                result.Add(new UserDto()
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    MiddleName = item.MiddleName,
                    Email = item.Email,
                    CarNumber = userTransport.Message.Transport != null ? userTransport.Message.Transport.Number : string.Empty,
                });
            }

            await context.RespondAsync<IGetUsersQueryResult>(new { Users = result.ToArray() });
        }
    }
}
