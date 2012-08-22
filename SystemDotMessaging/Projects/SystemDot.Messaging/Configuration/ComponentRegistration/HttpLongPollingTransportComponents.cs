using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class HttpLongPollingTransportComponents
    {
        public static void Register()
        {
            IocContainer.RegisterInstance<IMessageReciever, LongPollReciever>();
            IocContainer.RegisterInstance<IMessageSender, MessageSender>();
        }
    }
}