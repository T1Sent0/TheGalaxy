namespace TheGalaxy.Interfaces.Core.Auth.Command
{
    public interface IGetUserTokenQuery
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
