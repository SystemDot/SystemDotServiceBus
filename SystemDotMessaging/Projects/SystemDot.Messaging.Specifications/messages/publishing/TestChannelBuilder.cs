using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Specifications.messages.publishing
{
    public class TestChannelBuilder : IChannelBuilder
    {
        readonly SubscriptionSchema subscriptionSchema;
        readonly IMessageInputter<MessagePayload> subscriptionChannel;

        public TestChannelBuilder(
            SubscriptionSchema subscriptionSchema,
            IMessageInputter<MessagePayload> subscriptionChannel)
        {
            this.subscriptionSchema = subscriptionSchema;
            this.subscriptionChannel = subscriptionChannel;
        }

        public IMessageInputter<MessagePayload> Build(SubscriptionSchema toSchema)
        {
            if (subscriptionSchema != toSchema) return null; 
            return this.subscriptionChannel;
        }
    }
}