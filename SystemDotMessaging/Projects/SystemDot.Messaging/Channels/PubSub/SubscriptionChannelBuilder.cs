using SystemDot.Messaging.Channels.Building;
using SystemDot.Messaging.Channels.Messages.Distribution;

namespace SystemDot.Messaging.Channels.PubSub
{
    public class SubscriptionChannelBuilder : ISubscriptionChannelBuilder 
    {
        public IDistributionSubscriber Build(SubscriptionSchema toSchema) 
        { 
            ChannelBuilder.StartsWith(new SubscriberStartPoint());
            return null;
        }
    }
}