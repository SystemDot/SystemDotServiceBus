using SystemDot.Ioc;

namespace SystemDot.Messaging.Transport.InProcess.Configuration
{
    static class InProcessTransportComponents
    {
        public static void Register(IIocContainer iocContainer)
        {
            iocContainer.RegisterInstance<ITransportBuilder, InProcessTransportBuilder>();
            iocContainer.RegisterInstance<IInProcessMessageServer, InProcessMessageServer>();
            iocContainer.RegisterInstance<IMessageReceiver, MessageReceiver>();
            iocContainer.RegisterInstance<IMessageSender, MessageSender>();
        }
    }
}