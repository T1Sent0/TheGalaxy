namespace TheGalaxy.Interfaces.Core.Auth.Command
{
    public class GetUserTokenQueryResult : IGetUserTokenQueryResult
    {
        public GetUserTokenQueryResult(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken)) throw new ApplicationException("Токен не выдан");

            AccessToken = accessToken;
        }

        public string AccessToken { get; set; }
    }
}
