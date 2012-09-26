using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Publishing.Builders;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionRequestHandler : IMessageInputter<MessagePayload>
    {
        readonly IPublisherRegistry publisherRegistry;
        readonly ISubscriberSendChannelBuilder builder;

        public SubscriptionRequestHandler(IPublisherRegistry publisherRegistry, ISubscriberSendChannelBuilder builder)
        {
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(builder != null);
            
            this.publisherRegistry = publisherRegistry;
            this.builder = builder;
        }

        public void InputMessage(MessagePayload message)
        {
            EndpointAddress subscriberAddress = message.GetSubscriptionRequestSchema().SubscriberAddress;
            EndpointAddress fromAddress = message.GetToAddress();

            Logger.Info("Handling request reply subscription request for {0}", subscriberAddress);

            GetPublisher(fromAddress).Subscribe(
                subscriberAddress, 
                this.builder.BuildChannel(new SubscriberSendChannelSchema(fromAddress, subscriberAddress)));
        }

        IPublisher GetPublisher(EndpointAddress address)
        {
            return publisherRegistry.GetPublisher(address);
        }
    }
}