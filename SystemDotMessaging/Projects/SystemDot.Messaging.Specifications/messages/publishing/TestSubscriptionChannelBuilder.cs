using SystemDot.Messaging.Channels.Messages;
using SystemDot.Messaging.Channels.PubSub;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Specifications.messages.publishing
{
    public class TestSubscriptionChannelBuilder : ISubscriptionChannelBuilder
    {
        readonly SubscriptionSchema subscriptionSchema;
        readonly IMessageInputter<MessagePayload> subscriptionChannel;

        public TestSubscriptionChannelBuilder(
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