using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriptionRequestReceiveChannelBuilder
    {
        readonly MessageReceiver messageReceiver;
        readonly AcknowledgementSender acknowledgementSender;
        readonly IPublisherRegistry publisherRegistry;
        readonly ServerAddressRegistry serverAddressRegistry;
        readonly AuthenticationSessionCache authenticationSessionCache;
        readonly AuthenticatedServerRegistry authenticatedServerRegistry;

        SubscriptionRequestReceiveChannelBuilder(
            MessageReceiver messageReceiver, 
            AcknowledgementSender acknowledgementSender, 
            IPublisherRegistry publisherRegistry, 
            ServerAddressRegistry serverAddressRegistry, 
            AuthenticationSessionCache authenticationSessionCache, 
            AuthenticatedServerRegistry authenticatedServerRegistry)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(serverAddressRegistry != null);
            Contract.Requires(authenticationSessionCache != null);
            Contract.Requires(authenticatedServerRegistry != null);
            
            this.messageReceiver = messageReceiver;
            this.acknowledgementSender = acknowledgementSender;
            this.publisherRegistry = publisherRegistry;
            this.serverAddressRegistry = serverAddressRegistry;
            this.authenticationSessionCache = authenticationSessionCache;
            this.authenticatedServerRegistry = authenticatedServerRegistry;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(messageReceiver)
                .Pump()
                .ToProcessor(new SubscriptionRequestFilter())
                .ToProcessor(new ReceiverAuthenticationSessionVerifier(authenticationSessionCache, authenticatedServerRegistry))
                .ToProcessor(new MessageLocalAddressReassigner(serverAddressRegistry))
                .ToProcessor(new MessageAcknowledger(acknowledgementSender))
                .ToEndPoint(new SubscriptionRequestHandler(publisherRegistry));
        }
    }
}