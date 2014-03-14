using SystemDot.Configuration;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging.Configuration
{
    public class Configure : ConfigurationBase
    {
        public static MessagingConfiguration Messaging()
        {
            SystemDot.Configuration.Configure.SystemDot()
                .ResolveReferencesWith(IocContainerLocator.Locate())
                .Initialise();

            return new MessagingConfiguration();
        }
    }
}