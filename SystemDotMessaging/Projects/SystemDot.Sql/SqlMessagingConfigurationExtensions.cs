using SystemDot.Ioc;
using SystemDot.Sql;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Configuration
{
    public static class SqlMessagingConfigurationExtensions
    {
        public static MessagingConfiguration UsingSqlPersistence(this MessagingConfiguration configuration, string connection)
        {
            DbChangeStore.ConnectionString = connection;

            configuration.GetInternalIocContainer().RegisterInstance<ChangeStore, SqlChangeStore>();

            return configuration;
        }
    }
}