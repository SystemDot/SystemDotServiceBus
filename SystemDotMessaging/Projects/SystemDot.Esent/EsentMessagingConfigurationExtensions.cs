using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;
using SystemDot.Storage.Changes;

namespace SystemDot.Esent
{
    public static class EsentMessagingConfigurationExtensions
    {
        public static MessagingConfiguration UsingFilePersistence(this MessagingConfiguration configuration)
        {
            IIocContainer container = IocContainerLocator.Locate();

            container.RegisterInstance<IChangeStore, EsentChangeStore>();
            configuration.BuildActions.Add(() => container.Resolve<IChangeStore>().Initialise(string.Empty));

            return configuration;
        }
    }
}