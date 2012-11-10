using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Storage.Esent
{
    public static class EsentMessageServerConfigurationExtensions
    {
        public static MessageServerConfiguration UsingEsentPersistence(this MessageServerConfiguration configuration, string path)
        {
            IocContainerLocator.Locate().RegisterInstance<IChangeStore, EsentChangeStore>();
            IocContainerLocator.Locate().Resolve<IChangeStore>().Initialise(path);

            return configuration;
        }
    }
}