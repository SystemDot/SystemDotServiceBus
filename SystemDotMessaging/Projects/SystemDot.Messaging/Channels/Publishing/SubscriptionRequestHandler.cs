using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionRequestHandler : IMessageInputter<MessagePayload>
    {
        readonly IPublisherRegistry publisherRegistry;
        readonly IChannelBuilder channelBuilder;
        
        public SubscriptionRequestHandler(IPublisherRegistry publisherRegistry, IChannelBuilder channelBuilder)
        {
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(channelBuilder != null);

            this.publisherRegistry = publisherRegistry;
            this.channelBuilder = channelBuilder;
        }

        public void InputMessage(MessagePayload message)
        {
            if (!message.IsSubscriptionRequest()) return;

            Logger.Info("Handling request reply subscription request for {0}",
                message.GetSubscriptionRequestSchema().SubscriberAddress);

            GetPublisher(message).Subscribe(
                message.GetSubscriptionRequestSchema().SubscriberAddress, 
                this.channelBuilder.Build(message.GetSubscriptionRequestSchema()));
        }

        IDistributor GetPublisher(MessagePayload message)
        {
            return publisherRegistry.GetPublisher(message.GetToAddress());
        }
    }
}