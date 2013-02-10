using System.Diagnostics.Contracts;
using SystemDot.Ioc;

namespace SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration
{
    public static class HttpRemoteServerComponents
    {
        public static void Configure(IIocContainer container)
        {
            Contract.Requires(container != null);
            container.RegisterInstance<IMessageReciever, MessageReciever>();
            container.RegisterInstance<ITransportBuilder, HttpRemoteServerTransportBuilder>();
        }
    }
}