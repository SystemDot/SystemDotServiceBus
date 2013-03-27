using SystemDot.Http.Builders;
using SystemDot.Ioc;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    static class HttpTransportComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IMessageReceiver, MessageReceiver>();
            container.RegisterInstance<IHttpServerBuilder, HttpServerBuilder>();
            container.RegisterInstance<IMessageSender, MessageSender>();
        }
    }
}