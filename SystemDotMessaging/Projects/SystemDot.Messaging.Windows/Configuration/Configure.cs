
using SystemDot.Environment;

namespace SystemDot.Messaging.Configuration
{
    public class Configure : ConfigurationBase
    {
        public static MessagingConfiguration Messaging()
        {
            Components.Register();
            return new MessagingConfiguration();
        }
    }
}