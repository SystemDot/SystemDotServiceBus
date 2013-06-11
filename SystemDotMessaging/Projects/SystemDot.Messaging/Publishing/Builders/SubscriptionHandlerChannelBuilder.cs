using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriptionHandlerChannelBuilder
    {
        readonly IMessageReceiver messageReceiver;
        readonly AcknowledgementSender acknowledgementSender;
        readonly IPublisherRegistry publisherRegistry;
        readonly ServerAddressRegistry serverAddressRegistry;

        internal SubscriptionHandlerChannelBuilder(
            IMessageReceiver messageReceiver, 
            AcknowledgementSender acknowledgementSender, 
            IPublisherRegistry publisherRegistry, 
            ServerAddressRegistry serverAddressRegistry)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(serverAddressRegistry != null);
            
            this.messageReceiver = messageReceiver;
            this.acknowledgementSender = acknowledgementSender;
            this.publisherRegistry = publisherRegistry;
            this.serverAddressRegistry = serverAddressRegistry;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReceiver)
                .Pump()
                .ToProcessor(new SubscriptionRequestFilter())
                .ToProcessor(new MessageOriginServerAddressRegistrar(this.serverAddressRegistry))
                .ToProcessor(new MessageAcknowledger(this.acknowledgementSender))
                .ToEndPoint(new SubscriptionRequestHandler(this.publisherRegistry));
        }
    }
}