using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Channels.Publishing.Builders;
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

            EndpointAddress subscriberAddress = message.GetSubscriptionRequestSchema().SubscriberAddress;
            EndpointAddress fromAddress = message.GetToAddress();

            Logger.Info("Handling request reply subscription request for {0}", subscriberAddress);
            
            GetPublisher(fromAddress).Subscribe(subscriberAddress, BuildChannel(fromAddress, subscriberAddress));
        }

        IDistributor GetPublisher(EndpointAddress address)
        {
            return publisherRegistry.GetPublisher(address);
        }

        private IMessageInputter<MessagePayload> BuildChannel(EndpointAddress toAddress, EndpointAddress subscriberAddress)
        {
            return this.channelBuilder.Build(toAddress, subscriberAddress);
        }
    }
}