using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Transport.Http.Remote.Clients.Configuration;
using SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    public class HttpTransportConfiguration : Configurer
    {
        readonly List<Action> buildActions;

        public HttpTransportConfiguration(List<Action> buildActions)
        {
            Contract.Requires(buildActions != null);

            this.buildActions = buildActions;
        }

        public HttpTransportConfiguration AsARemoteServer(string instance)
        {
            Contract.Requires(!String.IsNullOrEmpty(instance));
            
            HttpRemoteServerComponents.Configure(IocContainerLocator.Locate());
            buildActions.Add(() => Resolve<HttpRemoteServerBuilder>().Build(instance));

            return this;
        }

        public void Initialise()
        {
            this.buildActions.ForEach(a => a());
        }

        public RemoteClientConfiguration AsARemoteClient(string instance)
        {
            Contract.Requires(!String.IsNullOrEmpty(instance)); 
            
            HttpRemoteClientComponents.Configure(IocContainerLocator.Locate());
            return new RemoteClientConfiguration(this.buildActions, MessageServer.Local(instance));
        }

        public MessageServerConfiguration AsAServer(string instance)
        {
            Contract.Requires(!String.IsNullOrEmpty(instance));

            HttpServerComponents.Configure(IocContainerLocator.Locate());

            return new MessageServerConfiguration(
                this.buildActions,
                new ServerPath(
                    MessageServer.Local(instance), 
                    MessageServer.Local(instance)));
        }
    }
}