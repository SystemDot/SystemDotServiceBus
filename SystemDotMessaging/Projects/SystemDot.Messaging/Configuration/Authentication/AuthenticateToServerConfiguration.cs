using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Configuration.Authentication
{
    public class AuthenticateToServerConfiguration
    {
        readonly MessagingConfiguration messagingConfiguration;
        readonly MessageServer server;
        readonly string serverRequiringAuthentication;

        public AuthenticateToServerConfiguration(MessagingConfiguration messagingConfiguration, MessageServer server, string serverRequiringAuthentication)
        {
            Contract.Requires(messagingConfiguration != null);
            Contract.Requires(server != null);
            Contract.Requires(!string.IsNullOrEmpty(serverRequiringAuthentication));
            
            this.messagingConfiguration = messagingConfiguration;
            this.server = server;
            this.serverRequiringAuthentication = serverRequiringAuthentication;
        }

        public AuthenticateToServerWithRequestConfiguration<T> WithRequest<T>()
        {
            return new AuthenticateToServerWithRequestConfiguration<T>(messagingConfiguration, server, serverRequiringAuthentication);
        }
    }
}