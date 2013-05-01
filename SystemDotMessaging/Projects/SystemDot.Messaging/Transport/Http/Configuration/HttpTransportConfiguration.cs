using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Transport.Http.Remote.Clients.Configuration;
using SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    public class HttpTransportConfiguration : ConfigurationBase
    {
        readonly MessagingConfiguration messagingConfiguration;

        public HttpTransportConfiguration(MessagingConfiguration messagingConfiguration)
        {
            Contract.Requires(messagingConfiguration != null);

            this.messagingConfiguration = messagingConfiguration;
        }

        public HttpTransportConfiguration AsARemoteServer(string instance)
        {
            Contract.Requires(!String.IsNullOrEmpty(instance));
            
            HttpRemoteServerComponents.Configure(IocContainerLocator.Locate());
            this.messagingConfiguration.BuildActions.Add(() => Resolve<HttpRemoteServerBuilder>().Build(instance));

            return this;
        }

        public void Initialise()
        {
            this.messagingConfiguration.BuildActions.ForEach(a => a());
        }

        public RemoteClientConfiguration AsARemoteClient(string instance)
        {
            Contract.Requires(!String.IsNullOrEmpty(instance)); 
            
            HttpRemoteClientComponents.Configure(IocContainerLocator.Locate());
            return new RemoteClientConfiguration(this.messagingConfiguration, MessageServer.Local(instance));
        }

        public MessageServerConfiguration AsAServer(string instance)
        {
            Contract.Requires(!String.IsNullOrEmpty(instance));

            HttpServerComponents.Configure(IocContainerLocator.Locate());

            return new MessageServerConfiguration(
                this.messagingConfiguration,
                new ServerPath(
                    MessageServer.Local(instance), 
                    MessageServer.Local(instance)));
        }
    }
}