using System;
using System.Collections.Generic;
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
    public class MessageServerConfiguration : Configurer
    {
        readonly ServerPath serverPath;
        readonly MessagingConfiguration messagingConfiguration;

        public MessageServerConfiguration(MessagingConfiguration messagingConfiguration, ServerPath serverPath)
        {
            Contract.Requires(messagingConfiguration != null);
            Contract.Requires(serverPath != null);

            this.serverPath = serverPath;
            this.messagingConfiguration = messagingConfiguration;

            IIocContainer container = IocContainerLocator.Locate();

            RegisterInMemoryPersistence(container);
            RegisterPlatformAgnosticSerialiser(container);
            ConfigureExternalSources(container);
        }

        public ChannelConfiguration OpenChannel(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            return new ChannelConfiguration(
                new EndpointAddress(name, this.serverPath),
                this.serverPath,
                this.messagingConfiguration.BuildActions);
        }

        public LocalChannelConfiguration OpenLocalChannel()
        {
            return new LocalChannelConfiguration(
                this.serverPath,
                this.messagingConfiguration.BuildActions);
        }

        static void RegisterInMemoryPersistence(IIocContainer container)
        {
            container.RegisterInstance<IChangeStore, InMemoryChangeStore>();
        }

        static void RegisterPlatformAgnosticSerialiser(IIocContainer container)
        {
            container.RegisterInstance<ISerialiser, PlatformAgnosticSerialiser>();
        }

        void ConfigureExternalSources(IIocContainer container)
        {
            container.Resolve<IExternalSourcesConfigurer>().Configure(this.messagingConfiguration, this);
        }
    }
}