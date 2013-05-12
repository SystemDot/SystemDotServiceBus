using SystemDot.Ioc;
using SystemDot.Sql;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Configuration
{
    public static class SqlMessagingConfigurationExtensions
    {
        public static MessagingConfiguration UsingSqlPersistence(
            this MessagingConfiguration configuration, 
            string connectionString)
        {
            IIocContainer container = configuration.GetIocContainer();

            container.RegisterInstance<IChangeStore, SqlChangeStore>();
            configuration.BuildActions.Add(() => container.Resolve<IChangeStore>().Initialise(connectionString));

            return configuration;
        }
    }
}