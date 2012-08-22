using SystemDot.Logging;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging.Configuration
{
    public class MessagingConfiguration
    {
        public MessageServerConfiguration UsingHttpTransport(MessageServer server)
        {
            HttpLongPollingTransportComponents.Register(IocContainerLocator.Locate());
            return new MessageServerConfiguration(server);
        }

        public MessageServerConfiguration UsingInProcessTransport()
        {
            InProcessTransportComponents.Register(IocContainerLocator.Locate());
            return new MessageServerConfiguration(MessageServer.Local());
        }

        public MessagingConfiguration LoggingWith(ILoggingMechanism toLogWith)
        {
            Logger.LoggingMechanism = toLogWith;
            return this;
        }
    }
}