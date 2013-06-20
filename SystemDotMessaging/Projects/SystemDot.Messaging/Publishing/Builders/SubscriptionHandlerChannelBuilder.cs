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

        internal SubscriptionHandlerChannelBuilder(
            IMessageReceiver messageReceiver, 
            AcknowledgementSender acknowledgementSender, 
            IPublisherRegistry publisherRegistry)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(publisherRegistry != null);
            
            this.messageReceiver = messageReceiver;
            this.acknowledgementSender = acknowledgementSender;
            this.publisherRegistry = publisherRegistry;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReceiver)
                .Pump()
                .ToProcessor(new SubscriptionRequestFilter())
                .ToProcessor(new MessageAcknowledger(this.acknowledgementSender))
                .ToEndPoint(new SubscriptionRequestHandler(this.publisherRegistry));
        }
    }
}