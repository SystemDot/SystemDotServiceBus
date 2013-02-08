using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;
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

        public Initialiser AsARemoteServer()
        {
            HttpRemoteServerComponents.Configure(IocContainerLocator.Locate());
            return new RemoteServerConfiguration(this.buildActions);
        }
    }
}