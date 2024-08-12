using TheGalaxy.Interfaces.Domain.Transport;

namespace TheGalaxy.Interfaces.Database.Transports.Queries
{
    public interface IDbGetTransportByIdQueryResult
    {
        public UserTransport Transport { get; set; }
    }
}
