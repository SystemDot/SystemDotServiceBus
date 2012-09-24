using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriptionHandlerChannelBuilder : ISubscriptionHandlerChannelBuilder
    {
        readonly IMessageReciever messageReciever;
        readonly IMessageSender messageSender;
        readonly IPublisherRegistry publisherRegistry;
        readonly ISubscriberSendChannelBuilder subscriberSendChannelBuilder;

        public SubscriptionHandlerChannelBuilder(
            IMessageReciever messageReciever, 
            IMessageSender messageSender, 
            IPublisherRegistry publisherRegistry, 
            ISubscriberSendChannelBuilder subscriberSendChannelBuilder)
        {
            Contract.Requires(messageReciever != null);
            Contract.Requires(messageSender != null);
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(subscriberSendChannelBuilder != null);
            
            this.messageReciever = messageReciever;
            this.messageSender = messageSender;
            this.publisherRegistry = publisherRegistry;
            this.subscriberSendChannelBuilder = subscriberSendChannelBuilder;
        }

        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new SubscriptionRequestChecker())
                .Pump()
                .ToProcessor(new MessageAcknowledger(this.messageSender))
                .ToEndPoint(new SubscriptionRequestHandler(this.publisherRegistry, this.subscriberSendChannelBuilder));
        }
    }
}