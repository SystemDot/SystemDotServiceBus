using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Builders;

namespace SystemDot.Messaging.Configuration.Authentication
{
    public class AuthenticatesOnReplyConfiguration<TAuthenticationRequest, TAuthenticationResponse> : Configurer
    {
        readonly MessageServer server;
        ExpiryPlan expiryPlan;

        public AuthenticatesOnReplyConfiguration(MessagingConfiguration messagingConfiguration, MessageServer server) : base(messagingConfiguration)
        {
            Contract.Requires(server != null);
            this.server = server;
            expiryPlan = ExpiryPlan.Never();
        }

        protected internal override void Build()
        {
            Resolve<AuthenticationReceiverBuilder>().Build<TAuthenticationRequest, TAuthenticationResponse>(this, server, expiryPlan);
        }

        protected override MessageServer GetMessageServer()
        {
            return server;
        }

        public AuthenticatesOnReplyConfiguration<TAuthenticationRequest, TAuthenticationResponse> Expires(ExpiryPlan expiry)
        {
            Contract.Requires(expiry != null);

            expiryPlan = expiry;
            return this;
        }
    }
}