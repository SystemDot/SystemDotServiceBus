using SystemDot.Newtonsoft;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Configuration
{
    public static class MessagingConfigurationExtensions
    {
        public static MessagingConfiguration UsingJsonSerialisation(this MessagingConfiguration config)
        {
            config.GetInternalIocContainer().RegisterInstance<ISerialiser, JsonSerialiser>();
            return config;
        } 
    }
}