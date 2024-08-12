using TheGalaxy.Interfaces.Models;

namespace TheGalaxy.Interfaces.Core.Transports.Queries
{
    public interface IGetTransportByNumberQueryResult
    {
        public UserTransportDto UserTransport { get; set; }
    }
}
