using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication.Builders;

namespace SystemDot.Messaging.Configuration.Authentication
{
    public class AuthenticateToServerWithRequestConfiguration<TAuthenticationRequest> : Configurer
    {
        readonly MessageServer server;
        readonly string serverRequiringAuthentication;

        public AuthenticateToServerWithRequestConfiguration(
            MessagingConfiguration messagingConfiguration,
            MessageServer server, 
            string serverRequiringAuthentication)
            : base(messagingConfiguration)
        {
            Contract.Requires(server != null);
            Contract.Requires(!string.IsNullOrEmpty(serverRequiringAuthentication));

            this.server = server;
            this.serverRequiringAuthentication = serverRequiringAuthentication;
        }

        protected internal override void Build()
        {
            Resolve<AuthenticationSenderBuilder>().Build<TAuthenticationRequest>(this, serverRequiringAuthentication);
        }

        protected override MessageServer GetMessageServer()
        {
            return server;
        }
    }
}