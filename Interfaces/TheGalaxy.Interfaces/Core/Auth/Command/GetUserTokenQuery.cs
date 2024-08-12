namespace TheGalaxy.Interfaces.Core.Auth.Command
{
    public class GetUserTokenQuery : IGetUserTokenQuery
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
