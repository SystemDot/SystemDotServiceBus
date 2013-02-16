using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.Transport.InProcess.Configuration
{
    public static class MessagingConfigurationExtensions
    {
        public static MessageServerConfiguration UsingInProcessTransport(this MessagingConfiguration config)
        {
            InProcessTransportComponents.Register(IocContainerLocator.Locate());
            return new MessageServerConfiguration(ServerPath.None);
        }

    }
}