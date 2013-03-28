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
        readonly List<Action> buildActions;

        public MessageServerConfiguration(List<Action> actions, ServerPath serverPath)
        {
            Contract.Requires(actions != null);
            Contract.Requires(serverPath != null);

            this.serverPath = serverPath;
            this.buildActions = actions;

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
                this.buildActions);
        }

        public LocalChannelConfiguration OpenLocalChannel()
        {
            return new LocalChannelConfiguration(
                this.serverPath, 
                this.buildActions);
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
            container.Resolve<IExternalSourcesConfigurer>().Configure(this);
        }
    }
}