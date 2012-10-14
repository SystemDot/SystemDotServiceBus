using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.Storage.Sqlite.Metro
{
    public static class SqliteMessageServerConfigurationExtensions
    {
        public static MessageServerConfiguration UsingSqlitePersistence(this MessageServerConfiguration configuration)
        {
            IIocContainer iocContainer = IocContainerLocator.Locate();

            iocContainer.RegisterInstance<IPersistenceFactory, SqlitePersistenceFactory>();
            
            return configuration;
        }
    }
}