using SystemDot.Http;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Transport.Http
{
    public static class MessageServerExtensions
    {
        public static FixedPortAddress GetUrl(this MessageServer server)
        {
            return new FixedPortAddress(server.Address.Path, server.Address.IsSecure, server.Name);
        }
    }
}