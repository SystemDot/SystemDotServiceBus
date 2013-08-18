using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Configuration.Authentication
{
    public class RequiresAuthenticationConfiguration
    {
        readonly MessagingConfiguration messagingConfiguration;
        readonly MessageServer server;

        public RequiresAuthenticationConfiguration(MessagingConfiguration messagingConfiguration, MessageServer server)
        {
            Contract.Requires(messagingConfiguration != null);
            Contract.Requires(server != null);
            
            this.messagingConfiguration = messagingConfiguration;
            this.server = server;
        }

        public AcceptsRequestConfiguration<T> AcceptsRequest<T>()
        {
            return new AcceptsRequestConfiguration<T>(messagingConfiguration, server);
        }
    }
}