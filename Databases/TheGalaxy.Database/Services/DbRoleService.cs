using MassTransit;

using TheGalaxy.Database.Repositories.Roles;
using TheGalaxy.Interfaces.Database.Roles.Commands;
using TheGalaxy.Interfaces.Database.Roles.Queries;
using TheGalaxy.Interfaces.Domain.Roles;

namespace TheGalaxy.Database.Services
{
    public class DbRoleService : IConsumer<IDbGetRoleByCodeQuery>, IConsumer<IDbCreateRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;

        public DbRoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task Consume(ConsumeContext<IDbGetRoleByCodeQuery> context)
        {
            var message = context.Message;

            var role = await _roleRepository.GetRoleByode(message.Code);

            await context.RespondAsync<IDbGetRoleByCodeQueryResult>(new { Role = role });
        }

        public async Task Consume(ConsumeContext<IDbCreateRoleCommand> context)
        {
            var message = context.Message;

            var roleId = await _roleRepository.CreateRole(message.Role);

            await context.RespondAsync<IDbCreateRoleCommandResult>(new { RoleId = roleId });
        }
    }
}
