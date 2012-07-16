using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Configuration
{
    public class Configure
    {
        public static MessageServerConfiguration WithLocalMessageServer()
        {
            return new MessageServerConfiguration();
        }
    }
}