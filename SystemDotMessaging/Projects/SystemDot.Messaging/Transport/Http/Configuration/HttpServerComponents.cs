using System.Diagnostics.Contracts;
using SystemDot.Ioc;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    static class HttpServerComponents
    {
        public static void Configure(IIocContainer container)
        {
            Contract.Requires(container != null);

            container.RegisterInstance<IMessageTransporter, MessageTransporter>();
            container.RegisterInstance<ITransportBuilder, HttpServerTransportBuilder>();
        }
    }
}