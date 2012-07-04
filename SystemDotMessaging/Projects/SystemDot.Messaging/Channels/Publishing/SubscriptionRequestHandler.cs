using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using System.Collections.Concurrent;
namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionRequestHandler : IMessageInputter<MessagePayload>
    {
        readonly IPublisherRegistry publisherRegistry;
        readonly ISubscriptionChannelBuilder channelBuilder;
        
        public SubscriptionRequestHandler(
            IPublisherRegistry publisherRegistry, 
            ISubscriptionChannelBuilder channelBuilder)
        {
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(channelBuilder != null);

            this.publisherRegistry = publisherRegistry;
            this.channelBuilder = channelBuilder;
        }

        public void InputMessage(MessagePayload message)
        {
            if (!message.IsSubscriptionRequest()) return;

            GetPublisher(message).Subscribe(
                message.GetSubscriptionRequestSchema().SubscriberAddress, 
                this.channelBuilder.Build(message.GetSubscriptionRequestSchema()));

        }

        IDistributor GetPublisher(MessagePayload message)
        {
            return this.publisherRegistry.GetPublisher(message.GetToAddress());
        }
    }
}