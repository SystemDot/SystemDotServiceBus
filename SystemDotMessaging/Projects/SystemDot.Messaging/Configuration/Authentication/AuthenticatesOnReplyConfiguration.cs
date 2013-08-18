using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication.Builders;

namespace SystemDot.Messaging.Configuration.Authentication
{
    public class AuthenticatesOnReplyConfiguration<TAuthenticationRequest, TAuthenticationResponse> : Configurer
    {
        readonly MessageServer server;

        public AuthenticatesOnReplyConfiguration(MessagingConfiguration messagingConfiguration, MessageServer server) : base(messagingConfiguration)
        {
            Contract.Requires(server != null);
            this.server = server;
        }

        protected internal override void Build()
        {
            Resolve<AuthenticationReceiverBuilder>().Build<TAuthenticationRequest, TAuthenticationResponse>(this, server);
        }

        protected override MessageServer GetMessageServer()
        {
            return server;
        }
    }
}