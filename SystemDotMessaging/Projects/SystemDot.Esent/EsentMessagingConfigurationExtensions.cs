using SystemDot.Ioc;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Esent;

namespace SystemDot.Messaging.Configuration
{
    public static class EsentMessagingConfigurationExtensions
    {
        public static MessagingConfiguration UsingFilePersistence(this MessagingConfiguration configuration)
        {
            IIocContainer container = configuration.GetInternalIocContainer();

            container.RegisterInstance<ChangeStore, EsentChangeStore>();

            return configuration;
        }
    }
}