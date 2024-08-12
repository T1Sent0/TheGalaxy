using TheGalaxy.Interfaces.Domain.Transport;

namespace TheGalaxy.Database.Repositories.Transports
{
    public interface ITransportRepository
    {
        Task<Guid> CreateTransport(UserTransport userTransport);
        Task<UserTransport?> GetById(Guid id);
        Task<UserTransport?> GetByNumber(string number);
    }
}
