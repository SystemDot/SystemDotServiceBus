using SystemDot.Logging;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Configuration.HttpMessaging;

namespace SystemDot.Messaging.Configuration
{
    public class MessagingConfiguration
    {
        public MessageServerConfiguration UsingHttpTransport(MessageServer server)
        {
            return new MessageServerConfiguration(server);
        }

        public MessagingConfiguration LoggingWith(ILoggingMechanism toLogWith)
        {
            Logger.LoggingMechanism = toLogWith;
            return this;
        }
    }
}