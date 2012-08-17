using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Specifications.messages.publishing
{
    public class TestChannelBuilder : IChannelBuilder
    {
        readonly EndpointAddress expectedFromAddress;
        readonly EndpointAddress expectedSubscriberAddress;
        readonly IMessageInputter<MessagePayload> subscriptionChannel;

        public TestChannelBuilder(
            EndpointAddress expectedFromAddress, 
            EndpointAddress expectedSubscriberAddress,
            IMessageInputter<MessagePayload> subscriptionChannel)
        {
            this.expectedFromAddress = expectedFromAddress;
            this.expectedSubscriberAddress = expectedSubscriberAddress;
            this.subscriptionChannel = subscriptionChannel;
        }

        public IMessageInputter<MessagePayload> Build(EndpointAddress fromAddress, EndpointAddress subscriberAddress)
        {
            if (fromAddress != this.expectedFromAddress) return null;
            if (subscriberAddress != this.expectedSubscriberAddress) return null; 
            return this.subscriptionChannel;
        }
    }
}