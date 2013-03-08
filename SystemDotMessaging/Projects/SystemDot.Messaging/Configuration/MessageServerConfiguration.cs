using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.ExternalSources;
using SystemDot.Messaging.Configuration.Local;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Configuration
{
    public class MessageServerConfiguration : Configurer
    {
        readonly ServerPath serverPath;
        readonly List<Action> buildActions;

        public MessageServerConfiguration(ServerPath serverPath)
        {
            Contract.Requires(serverPath != null);

            this.serverPath = serverPath;
            this.buildActions = new List<Action>();

            ConfigureExternalSources();
        }

        void ConfigureExternalSources()
        {
            IocContainerLocator.Locate().Resolve<IExternalSourcesConfigurer>().Configure(this);
        }

        public ChannelConfiguration OpenChannel(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            RegisterInMemoryPersistence(IocContainerLocator.Locate());

            return new ChannelConfiguration(
                new EndpointAddress(name, this.serverPath),
                this.serverPath,
                this.buildActions);
        }

        public LocalChannelConfiguration OpenLocalChannel()
        {
            RegisterInMemoryPersistence(IocContainerLocator.Locate());
            
            return new LocalChannelConfiguration(
                this.serverPath, 
                this.buildActions);
        }

        private static void RegisterInMemoryPersistence(IIocContainer container)
        {
            container.RegisterInstance<IChangeStore, InMemoryChangeStore>();
        }
    }
}