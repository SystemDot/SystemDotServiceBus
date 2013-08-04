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

            IIocContainer container = configuration.GetInternalIocContainer();

            RegisterChangeStore(container);
            SetChangeStoreInitialisationAsConfigBuildAction(configuration, container);

            return configuration;
        }

        static void RegisterChangeStore(IIocContainer container)
        {
            container.RegisterInstance<IChangeStore, SqlChangeStore>();
        }

        static void SetChangeStoreInitialisationAsConfigBuildAction(
            MessagingConfiguration configuration,
            IIocContainer container)
        {
            configuration.BuildActions.Add(() => container.Resolve<IChangeStore>().Initialise());
        }
    }
}