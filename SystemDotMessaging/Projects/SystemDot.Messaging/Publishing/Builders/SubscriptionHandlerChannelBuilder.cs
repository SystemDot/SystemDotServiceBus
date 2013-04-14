using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
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
        readonly ISerialiser serialiser;

        internal SubscriptionHandlerChannelBuilder(
            IMessageReceiver messageReceiver, 
            AcknowledgementSender acknowledgementSender, 
            IPublisherRegistry publisherRegistry, 
            ISerialiser serialiser)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(serialiser != null);
            
            this.messageReceiver = messageReceiver;
            this.acknowledgementSender = acknowledgementSender;
            this.publisherRegistry = publisherRegistry;
            this.serialiser = serialiser;
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