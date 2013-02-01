using SystemDot.Http;
using SystemDot.Messaging.Addressing;

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