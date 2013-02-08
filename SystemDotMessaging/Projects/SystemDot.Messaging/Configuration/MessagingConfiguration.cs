using SystemDot.Logging;

namespace SystemDot.Messaging.Configuration
{
    public class MessagingConfiguration
    {        
        public MessagingConfiguration LoggingWith(ILoggingMechanism toLogWith)
        {
            Logger.LoggingMechanism = toLogWith;
            return this;
        }
    }
}