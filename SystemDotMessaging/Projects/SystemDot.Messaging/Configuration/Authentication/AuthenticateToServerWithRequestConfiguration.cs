using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Builders;

namespace SystemDot.Messaging.Configuration.Authentication
{
    public class AuthenticateToServerWithRequestConfiguration<TAuthenticationRequest> : Configurer
    {
        readonly MessageServer server;
        readonly AuthenticationSenderSchema schema;

        public AuthenticateToServerWithRequestConfiguration(
            MessagingConfiguration messagingConfiguration,
            MessageServer server,
            string serverRequiringAuthentication)
            : base(messagingConfiguration)
        {
            Contract.Requires(server != null);
            Contract.Requires(!string.IsNullOrEmpty(serverRequiringAuthentication));

            this.server = server;
            this.schema = new AuthenticationSenderSchema
            {
                Server = serverRequiringAuthentication,
                ToRunOnExpiry = s => {}
            };
        }

        protected internal override void Build()
        {
            Resolve<AuthenticationSenderBuilder>().Build<TAuthenticationRequest>(this, schema);
        }

        protected override MessageServer GetMessageServer()
        {
            return server;
        }

        public AuthenticateToServerWithRequestConfiguration<TAuthenticationRequest> OnExpiry(
            Action<AuthenticationSession> toRunOnExpiry)
        {
            Contract.Requires(toRunOnExpiry != null);
            schema.ToRunOnExpiry = toRunOnExpiry;
            return this;
        }
    }
}