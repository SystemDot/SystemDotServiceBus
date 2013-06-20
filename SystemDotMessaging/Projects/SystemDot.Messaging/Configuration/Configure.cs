using SystemDot.Configuration;
using SystemDot.Messaging.Configuration.ComponentRegistration;

namespace SystemDot.Messaging.Configuration
{
    public class Configure : ConfigurationBase
    {
        public static MessagingConfiguration Messaging()
        {
            Components.Register();
            LoadConfigurationFromFile();

            return new MessagingConfiguration();
        }

        static void LoadConfigurationFromFile()
        {
            Resolve<IConfigurationReader>().Load("SystemDot.config");
        }
    }
}