using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Configuration.Authentication
{
    public class AcceptsRequestConfiguration<TAuthenticationRequest> 
    {
        readonly MessagingConfiguration messagingConfiguration;
        readonly MessageServer server;

        public AcceptsRequestConfiguration(MessagingConfiguration messagingConfiguration, MessageServer server)
        {
            Contract.Requires(messagingConfiguration != null);
            Contract.Requires(server != null);

            this.messagingConfiguration = messagingConfiguration;
            this.server = server;
        }

        public AuthenticatesOnReplyConfiguration<TAuthenticationRequest, TAuthenticationResponse> AuthenticatesOnReply<TAuthenticationResponse>()
        {
            return new AuthenticatesOnReplyConfiguration<TAuthenticationRequest, TAuthenticationResponse>(messagingConfiguration, server);
        }
    }
}