using TheGalaxy.Interfaces.Domain.Transport;

namespace TheGalaxy.Interfaces.Database.Transports.Commands
{
    public interface IDbCreateTransportCommand
    {
        public UserTransport UserTransport { get; set; }
    }
}
