using System.Diagnostics.Contracts;
using SystemDot.Ioc;

namespace SystemDot.Messaging.Transport.Http.Remote.Clients.Configuration
{
    public static class HttpRemoteClientComponents
    {
        public static void Configure(IIocContainer container)
        {
            Contract.Requires(container != null); 

            container.RegisterInstance<IMessageReciever, MessageReciever>();
            container.RegisterInstance<LongPoller, LongPoller>();
            container.RegisterInstance<ITransportBuilder, HttpRemoteClientTransportBuilder>();
        }
    }
}