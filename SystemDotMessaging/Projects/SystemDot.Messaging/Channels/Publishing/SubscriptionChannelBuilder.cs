using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionChannelBuilder : IMessageInputter<MessagePayload>
    {
        readonly IPublisherRegistry publisherRegistry;
        readonly IMessageInputter<MessagePayload> messageSender;

        public SubscriptionChannelBuilder(
            IPublisherRegistry publisherRegistry, 
            IMessageInputter<MessagePayload> messageSender)
        {
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(messageSender != null);

            this.publisherRegistry = publisherRegistry;
            this.messageSender = messageSender;
        }

        public void InputMessage(MessagePayload message)
        {
            EndpointAddress subscriberAddress = message.GetSubscriptionRequestSchema().SubscriberAddress;
            EndpointAddress fromAddress = message.GetToAddress();

            Logger.Info("Handling request reply subscription request for {0}", subscriberAddress);
            
            GetPublisher(fromAddress).Subscribe(subscriberAddress, BuildChannel(fromAddress, subscriberAddress));
        }

        IDistributor GetPublisher(EndpointAddress address)
        {
            return publisherRegistry.GetPublisher(address);
        }

        public IMessageInputter<MessagePayload> BuildChannel(EndpointAddress fromAddress, EndpointAddress subscriberAddress)
        {
            var addresser = new MessageAddresser(fromAddress, subscriberAddress);

            MessagePipelineBuilder.Build()
                .With(addresser)
                .ToEndPoint(this.messageSender);

            return addresser;
        }
    }
}