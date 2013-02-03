using SystemDot.Ioc;
using SystemDot.Messaging.Transport.Http.LongPolling;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    public static class HttpTransportComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IMessageReciever, LongPollReciever>();
            container.RegisterInstance<IMessageSender, MessageSender>();
        }
    }
}