using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Configuration.Authentication
{
    public class RequiresAuthenticationAcceptsRequestConfiguration<TAuthenticationRequest> : Configurer
    {
        readonly MessageServer server;

        public RequiresAuthenticationAcceptsRequestConfiguration(MessagingConfiguration messagingConfiguration, MessageServer server) : base(messagingConfiguration)
        {
            Contract.Requires(server != null);

            this.server = server;
        }

        protected internal override void Build()
        {
            OpenDirectChannel(ChannelNames.Authentication)
                .ForRequestReplyReceiving()
                    .OnlyForMessages(FilteredBy.Type<TAuthenticationRequest>());
        }

        protected override MessageServer GetMessageServer()
        {
            return server;
        }
    }
}