using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.Storage.Sql
{
    public static class SqlMessageServerConfigurationExtensions
    {
        public static MessageServerConfiguration UsingSqlPersistence(this MessageServerConfiguration configuration)
        {
            IocContainerLocator.Locate().RegisterInstance<IPersistenceFactory, SqlPersistenceFactory>();
            return configuration;
        }
    }
}