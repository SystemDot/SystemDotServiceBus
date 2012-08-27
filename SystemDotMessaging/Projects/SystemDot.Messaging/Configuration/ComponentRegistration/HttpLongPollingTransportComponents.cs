using SystemDot.Ioc;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class HttpLongPollingTransportComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IMessageReciever, LongPollReciever>();
            container.RegisterInstance<IMessageSender, MessageSender>();
        }
    }
}