namespace TheGalaxy.Interfaces.Database.Users.Queries
{
    public interface IDbGetUserByIdQuery
    {
        public Guid UserId { get; set; }
    }
}
