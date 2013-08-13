using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Configuration.Authentication
{
    public class AuthenticatToServerWithRequestConfiguration<TAuthenticationRequest> : Configurer
    {
        readonly MessageServer server;
        readonly string serverRequiringAuthentication;

        public AuthenticatToServerWithRequestConfiguration(
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
            OpenDirectChannel(ChannelNames.Authentication)
                .ForRequestReplySendingTo(GetAuthenticationReceiverChannel())
                    .OnlyForMessages(FilteredBy.Type<TAuthenticationRequest>());
        }

        string GetAuthenticationReceiverChannel()
        {
            return String.Concat(ChannelNames.Authentication, "@", serverRequiringAuthentication);
        }

        protected override MessageServer GetMessageServer()
        {
            return server;
        }
    }
}