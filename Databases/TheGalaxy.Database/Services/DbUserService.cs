using MassTransit;

using TheGalaxy.Database.Repositories.Users;
using TheGalaxy.Interfaces.Database.Users.Commands;
using TheGalaxy.Interfaces.Database.Users.Queries;

namespace TheGalaxy.Database.Services
{
    public class DbUserService : IConsumer<IDbCreateUserCommand>, IConsumer<IDbGetUserByEmailQuery>,
        IConsumer<IDbGetUserByIdQuery>, IConsumer<IDbUpdateUserCommand>, IConsumer<IDbGetUsersQuery>
    {
        private readonly IUserRepository _userRepository;

        public DbUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<IDbCreateUserCommand> context)
        {
            var message = context.Message;

            var createResult = await _userRepository.CreateUser(message.NewUser);

            await context.RespondAsync<IDbCreateUserCommandResult>(new { Succes = createResult });
        }

        public async Task Consume(ConsumeContext<IDbGetUserByEmailQuery> context)
        {
            var message = context.Message;

            var user = await _userRepository.GetUserByEmail(message.Email);

            await context.RespondAsync<IDbGetUserByEmailQueryResult>(new { User = user });
        }

        public async Task Consume(ConsumeContext<IDbGetUserByIdQuery> context)
        {
            var message = context.Message;

            var user = await _userRepository.GetUserById(message.UserId);

            await context.RespondAsync<IDbGetUserByIdQueryResult>(new { User = user });
        }

        public async Task Consume(ConsumeContext<IDbUpdateUserCommand> context)
        {
            var message = context.Message;

            var updateResult = await _userRepository.UpdateUser(message.User);

            await context.RespondAsync<IDbUpdateUserCommandResult>(new { Success = updateResult });
        }

        public async Task Consume(ConsumeContext<IDbGetUsersQuery> context)
        {
            var users = await _userRepository.GetUsers();
            await context.RespondAsync<IDbGetUsersQueryResult>(new { Users = users });
        }
    }
}
