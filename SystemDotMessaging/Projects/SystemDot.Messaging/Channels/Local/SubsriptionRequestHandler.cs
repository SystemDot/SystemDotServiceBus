using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Channels.Local
{
    public class SubsriptionRequestHandler : IDistributionSubscriber
    {
        readonly PublisherRegistry publisherRegistry;
        readonly IDistributionSubscriber subscriptionChannel;

        public SubsriptionRequestHandler(
            PublisherRegistry publisherRegistry, 
            IDistributionSubscriber subscriptionChannel)
        {
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(subscriptionChannel != null);

            this.publisherRegistry = publisherRegistry;
            this.subscriptionChannel = subscriptionChannel;
        }

        public void Recieve(MessagePayload message)
        {
            this.publisherRegistry.GetPublisher(message.Address).Subscribe(this.subscriptionChannel);
        }
    }
}