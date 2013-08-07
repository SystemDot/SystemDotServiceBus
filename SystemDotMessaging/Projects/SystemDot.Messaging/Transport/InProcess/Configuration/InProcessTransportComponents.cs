using SystemDot.Ioc;

namespace SystemDot.Messaging.Transport.InProcess.Configuration
{
    static class InProcessTransportComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IMessageTransporter, MessageTransporter>();
            container.RegisterInstance<IInProcessMessageServerFactory, InProcessMessageServerFactory>();
            container.RegisterInstance<ITransportBuilder, InProcessTransportBuilder>();
        }
    }
}