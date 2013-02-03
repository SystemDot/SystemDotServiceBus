using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;
using SystemDot.Storage.Changes;

namespace SystemDot.Esent
{
    public static class EsentMessageServerConfigurationExtensions
    {
        public static MessageServerConfiguration UsingFilePersistence(this MessageServerConfiguration configuration)
        {
            IocContainerLocator.Locate().RegisterInstance<IChangeStore, EsentChangeStore>();
            IocContainerLocator.Locate().Resolve<IChangeStore>().Initialise(string.Empty);

            return configuration;
        }
    }
}