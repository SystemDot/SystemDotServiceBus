using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.InProcess;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class InProcessTransportComponents
    {
        public static void Register()
        {
            IocContainer.RegisterInstance<IMessageReciever, MessageReciever>();
            IocContainer.RegisterInstance<IMessageSender, MessageSender>();
        }
    }
}