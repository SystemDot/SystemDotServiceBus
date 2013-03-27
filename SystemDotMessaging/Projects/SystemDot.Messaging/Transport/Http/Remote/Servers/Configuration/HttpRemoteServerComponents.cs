using System.Diagnostics.Contracts;
using SystemDot.Ioc;

namespace SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration
{
    static class HttpRemoteServerComponents
    {
        public static void Configure(IIocContainer container)
        {
            Contract.Requires(container != null);
            container.RegisterInstance<HttpRemoteServerBuilder, HttpRemoteServerBuilder>();
        }
    }
}