using MassTransit;

using Microsoft.AspNetCore.Mvc;

using TheGalaxy.Interfaces.Core.Auth.Command;

namespace TheGalaxyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRequestClient<IGetUserTokenQuery> _getUserTokenQueryRequestClient;

        public AuthController(IRequestClient<IGetUserTokenQuery> getUserTokenQueryRequestClient)
        {
            _getUserTokenQueryRequestClient = getUserTokenQueryRequestClient;
        }


        [HttpPost("[action]")]
        public async Task<GetUserTokenQueryResult> Authentificate([FromBody] GetUserTokenQuery tokenQuery)
        {
            var authResult = await _getUserTokenQueryRequestClient.GetResponse<IGetUserTokenQueryResult>(new { tokenQuery.Email, tokenQuery.Password });

            return new GetUserTokenQueryResult(authResult.Message.AccessToken);
        }
    }
}
