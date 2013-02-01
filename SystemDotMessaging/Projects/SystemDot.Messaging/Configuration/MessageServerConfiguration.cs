using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Configuration.Local;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Configuration
{
    public class MessageServerConfiguration : Configurer
    {
        readonly MessageServer messageServer;

        public MessageServerConfiguration(MessageServer messageServer)
        {
            Contract.Requires(messageServer != null);

            this.messageServer = messageServer;
        }

        public ChannelConfiguration OpenChannel(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            RegisterInMemoryPersistence(IocContainerLocator.Locate());

            return new ChannelConfiguration(
                BuildEndpointAddress(name, this.messageServer.Name),
                this.messageServer.Name,
                new List<Action>());
        }

        public LocalChannelConfiguration OpenLocalChannel()
        {
            RegisterInMemoryPersistence(IocContainerLocator.Locate());
            
            return new LocalChannelConfiguration(
                BuildEndpointAddress(string.Empty, this.messageServer.Name), 
                new List<Action>());
        }

        private static void RegisterInMemoryPersistence(IIocContainer container)
        {
            container.RegisterInstance<IChangeStore, InMemoryChangeStore>();
        }
    }
}