using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Messages.Distribution;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;

namespace SystemDot.Messaging.Channels.PubSub
{
    public class SubsriptionRequestHandler : IDistributionSubscriber
    {
        readonly IPublisherRegistry publisherRegistry;
        readonly ISubscriptionChannelBuilder channelBuilder;

        public SubsriptionRequestHandler(
            IPublisherRegistry publisherRegistry, 
            ISubscriptionChannelBuilder channelBuilder)
        {
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(channelBuilder != null);

            this.publisherRegistry = publisherRegistry;
            this.channelBuilder = channelBuilder;
        }

        public void Recieve(MessagePayload message)
        {
            if (!message.IsSubscriptionRequest()) return;

            IDistributor publisher = this.publisherRegistry.GetPublisher(message.Address);
            publisher.Subscribe(this.channelBuilder.Build(message.GetSubscriptionRequestSchema()));
        }
    }
}