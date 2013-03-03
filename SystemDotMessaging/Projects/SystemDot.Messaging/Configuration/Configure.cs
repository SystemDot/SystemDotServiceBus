using SystemDot.Ioc;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Configuration.ExternalSources;

namespace SystemDot.Messaging.Configuration
{
    public class Configure : Configurer
    {
        public static MessagingConfiguration Messaging()
        {
            Components.Register();

            var messagingConfiguration = new MessagingConfiguration();

            ConfigureExternalSources(messagingConfiguration);

            return messagingConfiguration;
        }

        static void ConfigureExternalSources(MessagingConfiguration messagingConfiguration)
        {
            IocContainerLocator.Locate().Resolve<IExternalSourcesConfigurer>().Configure(messagingConfiguration);
        }
    }
}