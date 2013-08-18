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

        public HttpTransportConfiguration AsAProxyFor(string instance)
        {
            Contract.Requires(!String.IsNullOrEmpty(instance));

            HttpRemoteServerComponents.Configure(IocContainerLocator.Locate());
            messagingConfiguration.BuildActions.Add(() => Resolve<HttpRemoteServerBuilder>().Build(instance));

            return this;
        }

        public MessageServerConfiguration AsAServerUsingAProxy(string server)
        {
            Contract.Requires(!String.IsNullOrEmpty(server));

            HttpRemoteClientComponents.Configure(IocContainerLocator.Locate());
            return new MessageServerConfiguration(messagingConfiguration, BuildServer(server));
        }

        public MessageServerConfiguration AsAServer(string server)
        {
            Contract.Requires(!String.IsNullOrEmpty(server));

            HttpServerComponents.Configure(IocContainerLocator.Locate());
            return new MessageServerConfiguration(messagingConfiguration, BuildServer(server));
        }

        static MessageServer BuildServer(string server)
        {
            return Resolve<MessageServerBuilder>().BuildInbound(server);
        }

        public void Initialise()
        {
            messagingConfiguration.BuildActions.ForEach(a => a());
        }
    }
}