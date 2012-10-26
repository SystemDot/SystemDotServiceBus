using SystemDot.Http;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;

namespace SystemDot.Messaging.Transport.Http
{
    public static class HttpAddressExtensions
    {
        public static FixedPortAddress GetUrl(this EndpointAddress address)
        {
            return new FixedPortAddress(address.ServerName);
        }
    }
}