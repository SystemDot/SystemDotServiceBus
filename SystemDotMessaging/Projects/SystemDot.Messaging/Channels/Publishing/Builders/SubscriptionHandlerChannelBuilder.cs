using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriptionHandlerChannelBuilder
    {
        readonly IMessageReciever messageReciever;
        readonly AcknowledgementSender acknowledgementSender;
        readonly IPublisherRegistry publisherRegistry;
        readonly ISerialiser serialiser;

        public SubscriptionHandlerChannelBuilder(
            IMessageReciever messageReciever, 
            AcknowledgementSender acknowledgementSender, 
            IPublisherRegistry publisherRegistry, 
            ISerialiser serialiser)
        {
            Contract.Requires(messageReciever != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(serialiser != null);
            
            this.messageReciever = messageReciever;
            this.acknowledgementSender = acknowledgementSender;
            this.publisherRegistry = publisherRegistry;
            this.serialiser = serialiser;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new MessagePayloadCopier(this.serialiser))
                .Pump()
                .ToProcessor(new SubscriptionRequestFilter())
                .ToProcessor(new MessageAcknowledger(this.acknowledgementSender))
                .ToEndPoint(new SubscriptionRequestHandler(this.publisherRegistry));
        }
    }
}