using MassTransit;

using Microsoft.Extensions.Configuration;

using TheGalaxy.Common;
using TheGalaxy.Common.Helpers;
using TheGalaxy.Interfaces.Core.Auth.Command;
using TheGalaxy.Interfaces.Database.Users.Queries;
using TheGalaxy.Interfaces.Enums.Enums;

namespace TheGalaxy.Core.Services.Auth
{
    public class AuthService : IConsumer<IGetUserTokenQuery>
    {
        private readonly IConfiguration _configuration;
        private readonly IRequestClient<IDbGetUserByEmailQuery> _getUserRequestClient;

        public AuthService(IConfiguration configuration, IRequestClient<IDbGetUserByEmailQuery> getUserRequestClient)
        {
            _getUserRequestClient = getUserRequestClient;
            _configuration = configuration;
        }

        public async Task Consume(ConsumeContext<IGetUserTokenQuery> context)
        {
            var message = context.Message;

            var findUser = await _getUserRequestClient.GetResponse<IDbGetUserByEmailQueryResult>(new { Email = message.Email });
            string userToken = null;
            if (findUser != null)
            {
                var hashPassword = PasswordHelper.StringToHash(message.Password, message.Email);
                if (hashPassword == findUser.Message.User.Password)
                {
                    userToken = TokenHelper.CreateUserAccessToken(_configuration, findUser.Message.User.Id, Enum.GetName(typeof(RoleEnum), findUser.Message.User.Role.Code));
                }
            }

            await context.RespondAsync<IGetUserTokenQueryResult>(new { AccessToken = userToken });
        }
    }
}
