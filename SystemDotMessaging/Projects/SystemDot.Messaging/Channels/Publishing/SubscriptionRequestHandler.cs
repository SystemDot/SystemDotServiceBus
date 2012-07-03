using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionRequestHandler : IMessageInputter<MessagePayload>
    {
        readonly IPublisherRegistry publisherRegistry;
        readonly ISubscriptionChannelBuilder channelBuilder;
        readonly Dictionary<EndpointAddress, List<EndpointAddress>> subscriptions;

        public SubscriptionRequestHandler(
            IPublisherRegistry publisherRegistry, 
            ISubscriptionChannelBuilder channelBuilder)
        {
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(channelBuilder != null);

            this.publisherRegistry = publisherRegistry;
            this.channelBuilder = channelBuilder;
            this.subscriptions = new Dictionary<EndpointAddress, List<EndpointAddress>>();
        }

        public void InputMessage(MessagePayload message)
        {
            if (!message.IsSubscriptionRequest()) return;

            if (PublisherSubscriptionExists(message)) return;

            GetPublisher(message)
                .Subscribe(this.channelBuilder.Build(message.GetSubscriptionRequestSchema()));

            AddPublisherSubscriber(message);
        }

        IDistributor GetPublisher(MessagePayload message)
        {
            return this.publisherRegistry.GetPublisher(message.GetToAddress());
        }

        bool PublisherSubscriptionExists(MessagePayload message)
        {
            if (GetPublisherSubcriptions(message.GetToAddress())
                .Any(s => s == message.GetSubscriptionRequestSchema().SubscriberAddress))
                return true;
            return false;
        }

        void AddPublisherSubscriber(MessagePayload message)
        {
            GetPublisherSubcriptions(message.GetToAddress())
                .Add(message.GetSubscriptionRequestSchema().SubscriberAddress);
        }

        List<EndpointAddress> GetPublisherSubcriptions(EndpointAddress publisherAddress)
        {
            if (!this.subscriptions.ContainsKey(publisherAddress))
                this.subscriptions[publisherAddress] = new List<EndpointAddress>();

            return this.subscriptions[publisherAddress];
        }
    }
}