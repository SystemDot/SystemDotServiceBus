using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Configuration
{
    public static class EsentMessagingConfigurationExtensions
    {
        public static MessagingConfiguration UsingFilePersistence(this MessagingConfiguration configuration)
        {
            IIocContainer container = configuration.GetIocContainer();

            container.RegisterInstance<IChangeStore, EsentChangeStore>();
            configuration.BuildActions.Add(() => container.Resolve<IChangeStore>().Initialise(string.Empty));

            return configuration;
        }
    }
}