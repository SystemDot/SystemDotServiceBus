using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Builders;

namespace SystemDot.Messaging.Configuration.Authentication
{
    public class AuthenticatesOnReplyConfiguration<TAuthenticationRequest, TAuthenticationResponse> : Configurer
    {
        readonly MessageServer server;
        readonly AuthenticationReceiverSchema schema;

        public AuthenticatesOnReplyConfiguration(MessagingConfiguration messagingConfiguration, MessageServer server) : base(messagingConfiguration)
        {
            Contract.Requires(server != null);

            this.server = server;

            schema = new AuthenticationReceiverSchema
            {
                ExpiresAfter = TimeSpan.MaxValue,
                ToRunOnExpiry = _ => { }
            };
        }

        protected internal override void Build()
        {
            Resolve<AuthenticationReceiverBuilder>().Build<TAuthenticationRequest, TAuthenticationResponse>(this, server, schema);
        }

        protected override MessageServer GetMessageServer()
        {
            return server;
        }

        public AuthenticatesOnReplyConfiguration<TAuthenticationRequest, TAuthenticationResponse> ExpiresAfter(TimeSpan after)
        {
            Contract.Requires(after != null);

            schema.ExpiresAfter = after;
            return this;
        }

        public AuthenticatesOnReplyConfiguration<TAuthenticationRequest, TAuthenticationResponse> OnExpiry(Action<AuthenticationSession> toRunOnExpiry)
        {
            schema.ToRunOnExpiry = toRunOnExpiry;
            return this;
        }

        public AuthenticatesOnReplyConfiguration<TAuthenticationRequest, TAuthenticationResponse> BlockMessagesIf(bool shouldBlock)
        {
            schema.BlockMessages = shouldBlock;
            return this;
        }
    }
}