using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    public static class MessagingConfigurationExtensions
    {
        public static MessageServerConfiguration UsingHttpTransport(this MessagingConfiguration config, MessageServer server)
        {
            HttpTransportComponents.Register(IocContainerLocator.Locate());
            return new MessageServerConfiguration(server);
        }
    }
}