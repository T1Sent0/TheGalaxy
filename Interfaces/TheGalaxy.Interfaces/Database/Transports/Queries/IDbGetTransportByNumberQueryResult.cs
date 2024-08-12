using TheGalaxy.Interfaces.Domain.Transport;

namespace TheGalaxy.Interfaces.Database.Transports.Queries
{
    public interface IDbGetTransportByNumberQueryResult
    {
        public UserTransport Transport { get; set; }
    }
}
