using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;
using SystemDot.Storage.Changes;

namespace SystemDot.Sql
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