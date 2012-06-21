using SystemDot.Messaging.Channels.Messages.Distribution;
using SystemDot.Messaging.Channels.PubSub;

namespace SystemDot.Messaging.Specifications.publishing
{
    public class TestSubscriptionChannelBuilder : ISubscriptionChannelBuilder
    {
        readonly SubscriptionSchema subscriptionSchema;
        readonly IDistributionSubscriber subscriptionChannel;

        public TestSubscriptionChannelBuilder(
            SubscriptionSchema subscriptionSchema, 
            IDistributionSubscriber subscriptionChannel)
        {
            this.subscriptionSchema = subscriptionSchema;
            this.subscriptionChannel = subscriptionChannel;
        }

        public IDistributionSubscriber Build(SubscriptionSchema toSchema)
        {
            if (subscriptionSchema != toSchema) return null; 
            return this.subscriptionChannel;
        }
    }
}