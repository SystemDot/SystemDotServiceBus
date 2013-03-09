using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;
using SystemDot.Serialisation;

namespace SystemDot.Newtonsoft
{
    public static class MessagingConfigurationExtensions
    {
        public static MessagingConfiguration UsingJsonSerialisation(this MessagingConfiguration config)
        {
            IocContainerLocator.Locate().RegisterInstance<ISerialiser, JsonSerialiser>();
            return config;
        } 
    }
}