using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.Http.Remote.Clients.Configuration;
using SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    public class HttpTransportConfiguration
    {
        readonly List<Action> buildActions;

        public HttpTransportConfiguration(List<Action> buildActions)
        {
            Contract.Requires(buildActions != null);
            this.buildActions = buildActions;
        }

        public RemoteServerConfiguration AsARemoteServer()
        {
            HttpRemoteServerComponents.Configure(IocContainerLocator.Locate());
            return new RemoteServerConfiguration(this.buildActions, new ServerPath());
        }

        public MessageServerConfiguration AsARemoteClientOf(MessageServer server)
        {
            HttpRemoteClientComponents.Configure(IocContainerLocator.Locate());
            return new MessageServerConfiguration(new ServerPath(server, server));
        }

        public MessageServerConfiguration AsAServer()
        {
            HttpServerComponents.Configure(IocContainerLocator.Locate());
            return new MessageServerConfiguration(new ServerPath(null, MessageServer.Local()));
        }
    }
}