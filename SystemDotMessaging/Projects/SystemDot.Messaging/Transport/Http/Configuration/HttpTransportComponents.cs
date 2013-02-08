using SystemDot.Http.Builders;
using SystemDot.Ioc;
using SystemDot.Messaging.Transport.Http.Remote;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    public static class HttpTransportComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IHttpServerBuilder, HttpServerBuilder>();
            container.RegisterInstance<IMessageReciever, LongPollReciever>();
            container.RegisterInstance<IMessageSender, MessageSender>();
        }
    }
}