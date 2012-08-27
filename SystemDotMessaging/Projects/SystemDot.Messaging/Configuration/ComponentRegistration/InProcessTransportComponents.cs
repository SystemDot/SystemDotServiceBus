using SystemDot.Ioc;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.InProcess;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class InProcessTransportComponents
    {
        public static void Register(IIocContainer iocContainer)
        {
            iocContainer.RegisterInstance<InProcessMessageServer, InProcessMessageServer>();
            iocContainer.RegisterInstance<IMessageReciever, MessageReciever>();
            iocContainer.RegisterInstance<IMessageSender, MessageSender>();
        }
    }
}