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

        public HttpTransportConfiguration AsAProxy(string instance)
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

        public MessageServerConfiguration AsAServerUsingAProxy(string server, string proxy)
        {
            Contract.Requires(!String.IsNullOrEmpty(server));
            Contract.Requires(!String.IsNullOrEmpty(proxy)); 
            
            HttpRemoteClientComponents.Configure(IocContainerLocator.Locate());

            return new MessageServerConfiguration(
                this.messagingConfiguration,
                Resolve<ServerPathBuilder>().Build(String.Concat(server, ".", Environment.MachineName), proxy));
        }

        public MessageServerConfiguration AsAServer(string name)
        {
            Contract.Requires(!String.IsNullOrEmpty(name));

            HttpServerComponents.Configure(IocContainerLocator.Locate());

            return new MessageServerConfiguration(
                this.messagingConfiguration,
                Resolve<ServerPathBuilder>().Build(name, name));
        }
    }
}