using System.Security.Policy;
using SystemDot.Http;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

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