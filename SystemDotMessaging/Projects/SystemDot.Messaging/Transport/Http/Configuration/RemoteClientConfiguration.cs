using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    public class RemoteClientConfiguration
    {
        readonly MessagingConfiguration messagingConfiguration;
        readonly MessageServer remoteClient;

        public RemoteClientConfiguration(MessagingConfiguration messagingConfiguration, MessageServer remoteClient)
        {
            Contract.Requires(remoteClient != null);
            Contract.Requires(messagingConfiguration != null);

            this.messagingConfiguration = messagingConfiguration;
            this.remoteClient = remoteClient;
        }

        public MessageServerConfiguration UsingProxy(string proxyName)
        {
            Contract.Requires(!string.IsNullOrEmpty(proxyName));

            return new MessageServerConfiguration(
                this.messagingConfiguration, 
                new ServerPath(this.remoteClient, MessageServer.Named(proxyName)));
        }
    }
}