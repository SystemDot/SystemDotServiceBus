using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Configuration
{
    public static class EsentMessagingConfigurationExtensions
    {
        public static MessagingConfiguration UsingFilePersistence(this MessagingConfiguration configuration)
        {
            IIocContainer container = configuration.GetInternalIocContainer();

            container.RegisterInstance<IChangeStore, EsentChangeStore>();

            return configuration;
        }
    }
}