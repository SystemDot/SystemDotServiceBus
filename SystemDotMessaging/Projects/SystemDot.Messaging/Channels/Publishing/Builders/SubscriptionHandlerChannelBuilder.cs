using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriptionHandlerChannelBuilder
    {
        readonly IMessageReciever messageReciever;
        readonly IMessageSender messageSender;
        readonly IPublisherRegistry publisherRegistry;
        
        public SubscriptionHandlerChannelBuilder(
            IMessageReciever messageReciever, 
            IMessageSender messageSender, 
            IPublisherRegistry publisherRegistry)
        {
            Contract.Requires(messageReciever != null);
            Contract.Requires(messageSender != null);
            Contract.Requires(publisherRegistry != null);
            
            this.messageReciever = messageReciever;
            this.messageSender = messageSender;
            this.publisherRegistry = publisherRegistry;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new SubscriptionRequestFilter())
                .Pump()
                .ToProcessor(new MessageAcknowledger(this.messageSender))
                .ToEndPoint(new SubscriptionRequestHandler(this.publisherRegistry));
        }
    }
}