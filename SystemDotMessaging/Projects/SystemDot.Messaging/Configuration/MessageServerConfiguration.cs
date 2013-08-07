using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.Direct;
using SystemDot.Messaging.Configuration.ExternalSources;
using SystemDot.Messaging.Configuration.Local;
using SystemDot.Messaging.Ioc;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Configuration
{
    public class MessageServerConfiguration : ConfigurationBase
    {
        readonly MessageServer server;
        readonly MessagingConfiguration messagingConfiguration;

        public MessageServerConfiguration(MessagingConfiguration messagingConfiguration, MessageServer server)
        {
            Contract.Requires(messagingConfiguration != null);
            Contract.Requires(server != null);

            this.server = server;
            this.messagingConfiguration = messagingConfiguration;

            IIocContainer container = IocContainerLocator.Locate();

            RegisterInMemoryPersistence(container);
            RegisterJsonSerialiser(container);
            ConfigureExternalSources(container);
        }

        public ChannelConfiguration OpenChannel(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            return new ChannelConfiguration(new EndpointAddress(name, server), server, messagingConfiguration);
        }

        public DirectChannelConfiguration OpenDirectChannel(string name) 
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            return new DirectChannelConfiguration(messagingConfiguration, BuildEndpointAddress(name, MessageServer.None));
        }

        public LocalChannelConfiguration OpenLocalChannel()
        {
            return new LocalChannelConfiguration(server, messagingConfiguration);
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
            container.Resolve<IExternalSourcesConfigurer>().Configure(messagingConfiguration, this);
        }
    }
}