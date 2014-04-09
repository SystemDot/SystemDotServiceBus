using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Sql;
using SystemDot.Storage.Changes.SqlServer;

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