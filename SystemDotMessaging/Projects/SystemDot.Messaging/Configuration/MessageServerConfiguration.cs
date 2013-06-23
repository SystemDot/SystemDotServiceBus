using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.ExternalSources;
using SystemDot.Messaging.Configuration.Local;
using SystemDot.Messaging.Ioc;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Configuration
{
    public class MessageServerConfiguration : ConfigurationBase
    {
        readonly ServerRoute serverRoute;
        readonly MessagingConfiguration messagingConfiguration;

        public MessageServerConfiguration(MessagingConfiguration messagingConfiguration, ServerRoute serverRoute)
        {
            Contract.Requires(messagingConfiguration != null);
            Contract.Requires(serverRoute != null);

            this.serverRoute = serverRoute;
            this.messagingConfiguration = messagingConfiguration;

            IIocContainer container = IocContainerLocator.Locate();

            RegisterInMemoryPersistence(container);
            RegisterJsonSerialiser(container);
            ConfigureExternalSources(container);
        }

        public ChannelConfiguration OpenChannel(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            return new ChannelConfiguration(
                new EndpointAddress(name, this.serverRoute),
                this.serverRoute,
                this.messagingConfiguration);
        }

        public LocalChannelConfiguration OpenLocalChannel()
        {
            return new LocalChannelConfiguration(
                this.serverRoute,
                this.messagingConfiguration);
        }

        static void RegisterInMemoryPersistence(IIocContainer container)
        {
            container.RegisterInstance<IChangeStore, InMemoryChangeStore>();
        }

        static void RegisterJsonSerialiser(IIocContainer container)
        {
            container.RegisterInstance<ISerialiser, JsonSerialiser>();
        }

        void ConfigureExternalSources(IIocContainer container)
        {
            container.Resolve<IExternalSourcesConfigurer>().Configure(this.messagingConfiguration, this);
        }
    }
}