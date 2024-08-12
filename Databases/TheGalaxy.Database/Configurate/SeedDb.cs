using MassTransit;

using TheGalaxy.Common.Helpers;
using TheGalaxy.Interfaces.Domain.Users;
using TheGalaxy.Interfaces.Database.Users.Commands;
using TheGalaxy.Interfaces.Database.Users.Queries;
using TheGalaxy.Interfaces.Domain.Roles;
using TheGalaxy.Interfaces.Enums.Enums;
using TheGalaxy.Interfaces.Database.Roles.Commands;
using TheGalaxy.Interfaces.Database.Roles.Queries;


namespace TheGalaxy.Database.Configurate
{
    public class SeedDb
    {
        private readonly IRequestClient<IDbCreateUserCommand> _dbCreateUserRequestClient;
        private readonly IRequestClient<IDbGetUserByEmailQuery> _dbGetUserByEmailRequestClient;
        private readonly IRequestClient<IDbCreateRoleCommand> _dbCreateRoleRequestClient;
        private readonly IRequestClient<IDbGetRoleByCodeQuery> _dbGetRoleByCodeRequestClient;
        public SeedDb(IRequestClient<IDbCreateUserCommand> dbCreateUserRequestClient,
            IRequestClient<IDbGetUserByEmailQuery> dbGetUserByEmailRequestClient,
            IRequestClient<IDbCreateRoleCommand> dbCreateRoleRequestClient,
            IRequestClient<IDbGetRoleByCodeQuery> dbGetRoleByCodeRequestClient)
        {
            _dbCreateUserRequestClient = dbCreateUserRequestClient;
            _dbGetUserByEmailRequestClient = dbGetUserByEmailRequestClient;
            _dbCreateRoleRequestClient = dbCreateRoleRequestClient;
            _dbGetRoleByCodeRequestClient = dbGetRoleByCodeRequestClient;
        }

        public async Task InitRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Code = RoleEnum.Support,
                    Name = "Слжба поддержки",
                    Description = "Поддержка пользователей"
                },
                new Role()
                {
                    Code = RoleEnum.Driver,
                    Name = "Водитель",
                    Description = "Сотрудник компании"
                }
            };

            foreach (var item in roles)
            {
                await _dbCreateRoleRequestClient.GetResponse<IDbCreateRoleCommandResult>(new { Role = item });
            }
        }

        public async Task InitUsers()
        {
            var supportRole = await _dbGetRoleByCodeRequestClient.GetResponse<IDbGetRoleByCodeQueryResult>(new { Code = RoleEnum.Support });
            var supportDriver = await _dbGetRoleByCodeRequestClient.GetResponse<IDbGetRoleByCodeQueryResult>(new { Code = RoleEnum.Driver });
            var users = new List<User>()
            {
                new User()
                {
                    FirstName = "Иван",
                    LastName = "Иванов",
                    MiddleName = "Иванович",
                    Email = "ivanov@thegalaxy.ru",
                    RoleId = supportRole.Message.Role.Id,
                    Password = PasswordHelper.Generate()
                },
                new User()
                {
                    FirstName = "Пётр",
                    LastName = "Петров",
                    MiddleName = "Петрович",
                    Email = "petrov@thegalaxy.ru",
                    RoleId = supportRole.Message.Role.Id,
                    Password = PasswordHelper.Generate()
                },
                new User()
                {
                    FirstName = "Иван",
                    LastName = "Сидоров",
                    MiddleName = "Петрович",
                    Email = "sidorov@thegalaxy.ru",
                    RoleId = supportDriver.Message.Role.Id,
                    Password = PasswordHelper.Generate()
                },
                new User()
                {
                    FirstName = "Мария",
                    LastName = "Рыбак",
                    MiddleName = "Игоревна",
                    Email = "ribak@thegalaxy.ru",
                    RoleId = supportDriver.Message.Role.Id,
                    Password = PasswordHelper.Generate()
                }
            };

            foreach (var item in users)
            {
                var existUser = await _dbGetUserByEmailRequestClient.GetResponse<IDbGetUserByEmailQueryResult>(new { item.Email });
                if (existUser.Message.User != null) continue;

                item.Password = PasswordHelper.StringToHash(item.Password, item.Email);

                await _dbCreateUserRequestClient.GetResponse<IDbCreateUserCommandResult>(new
                {
                    NewUser = item
                });
            }
        }
    }
}
