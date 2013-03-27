using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    public class RemoteClientConfiguration
    {
        readonly List<Action> buildActions;
        readonly MessageServer remoteClient;

        public RemoteClientConfiguration(List<Action> buildActions, MessageServer remoteClient)
        {
            Contract.Requires(remoteClient != null);
            Contract.Requires(buildActions != null);

            this.buildActions = buildActions;
            this.remoteClient = remoteClient;
        }

        public MessageServerConfiguration UsingProxy(MessageServer proxy)
        {
            Contract.Requires(proxy != null);
            return new MessageServerConfiguration(this.buildActions, new ServerPath(this.remoteClient, proxy));
        }
    }
}