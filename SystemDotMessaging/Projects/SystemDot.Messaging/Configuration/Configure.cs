using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Configuration
{
    public class Configure
    {
        public static LocalMessageServerConfiguration WithLocalMessageServer()
        {
            return new LocalMessageServerConfiguration();
        }
    }
}