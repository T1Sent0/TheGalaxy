using TheGalaxy.Interfaces.Models;

namespace TheGalaxy.Interfaces.Core.Transports.Commands
{
    public interface ICreatreTransportCommand
    {
        public UserTransportDto UserTransport { get; set; }
    }
}
