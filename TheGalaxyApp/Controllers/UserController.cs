using MassTransit;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TheGalaxy.Interfaces.Core.Users.Commands;
using TheGalaxy.Interfaces.Core.Users.Queries;
using TheGalaxy.Interfaces.Models;


namespace TheGalaxyApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IRequestClient<IGetUsersQuery> _getUsersQueryRequestClient;
        private readonly IRequestClient<IUpdateUserCommand> _updateUserCommandClient;

        public UserController(IRequestClient<IGetUsersQuery> getUsersQueryRequestClient,
            IRequestClient<IUpdateUserCommand> updateUserCommandClient)
        {
            _getUsersQueryRequestClient = getUsersQueryRequestClient;
            _updateUserCommandClient = updateUserCommandClient;
        }

        [HttpGet("[action]")]
        public async Task<UserDto[]> GetUsers()
        {
            var usersList = await _getUsersQueryRequestClient.GetResponse<IGetUsersQueryResult>(new {});

            return usersList.Message.Users;
        }


        [HttpPost("[action]")]
        public async Task UpdateUser([FromBody] UserDto user)
        {
            await _updateUserCommandClient.GetResponse<IGetUsersQueryResult>(new { User = user });
        }
    }
}
