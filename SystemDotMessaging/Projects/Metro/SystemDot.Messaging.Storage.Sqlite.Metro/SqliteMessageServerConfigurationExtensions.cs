using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Storage.Sqlite.Metro
{
    public static class SqliteMessageServerConfigurationExtensions
    {
        public static MessageServerConfiguration UsingSqlitePersistence(this MessageServerConfiguration configuration, string databasePath)
        {
            IocContainerLocator.Locate().RegisterInstance<IChangeStore, SqliteChangeStore>();
            IocContainerLocator.Locate().Resolve<IChangeStore>().Initialise(databasePath);
            
            return configuration;
        }
    }
}