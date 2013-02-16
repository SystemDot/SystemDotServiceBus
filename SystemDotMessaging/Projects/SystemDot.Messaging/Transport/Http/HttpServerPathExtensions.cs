using SystemDot.Http;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Transport.Http
{
    public static class HttpServerPathExtensions
    {
        public static FixedPortAddress GetUrl(this ServerPath serverPath)
        {
            return new FixedPortAddress(serverPath.Proxy.Name, serverPath.Proxy.Instance);
        }
    }
}