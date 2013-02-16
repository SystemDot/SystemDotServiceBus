using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    public class RemoteClientConfiguration
    {
        readonly MessageServer remoteClient;

        public RemoteClientConfiguration(MessageServer remoteClient)
        {
            Contract.Requires(remoteClient != null);
            this.remoteClient = remoteClient;
        }

        public MessageServerConfiguration UsingProxy(MessageServer proxy)
        {
            Contract.Requires(proxy != null);
            return new MessageServerConfiguration(new ServerPath(this.remoteClient, proxy));
        }
    }
}