using SystemDot.Messaging.Configuration.ComponentRegistration;

namespace SystemDot.Messaging.Configuration
{
    public class Configure : Configurer
    {
        public static MessagingConfiguration Messaging()
        {
            return new MessagingConfiguration();
        }
    }
}