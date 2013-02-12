using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.Local;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Configuration
{
    public class MessageServerConfiguration : Configurer
    {
        readonly ServerPath serverPath;

        public MessageServerConfiguration(ServerPath serverPath)
        {
            Contract.Requires(serverPath != null);

            this.serverPath = serverPath;
        }

        public ChannelConfiguration OpenChannel(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            RegisterInMemoryPersistence(IocContainerLocator.Locate());

            return new ChannelConfiguration(
                new EndpointAddress(name, this.serverPath),
                this.serverPath,
                new List<Action>());
        }

        public LocalChannelConfiguration OpenLocalChannel()
        {
            RegisterInMemoryPersistence(IocContainerLocator.Locate());
            
            return new LocalChannelConfiguration(
                this.serverPath, 
                new List<Action>());
        }

        private static void RegisterInMemoryPersistence(IIocContainer container)
        {
            container.RegisterInstance<IChangeStore, InMemoryChangeStore>();
        }
    }
}