using System.Diagnostics.Contracts;
using SystemDot.Ioc;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    public static class HttpServerComponents
    {
        public static void Configure(IIocContainer container)
        {
            Contract.Requires(container != null); 

            container.RegisterInstance<IMessageReciever, MessageReciever>();
            container.RegisterInstance<ITransportBuilder, HttpServerTransportBuilder>();
        }
    }
}