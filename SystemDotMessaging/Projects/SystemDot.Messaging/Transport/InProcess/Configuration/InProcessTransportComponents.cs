using SystemDot.Ioc;

namespace SystemDot.Messaging.Transport.InProcess.Configuration
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