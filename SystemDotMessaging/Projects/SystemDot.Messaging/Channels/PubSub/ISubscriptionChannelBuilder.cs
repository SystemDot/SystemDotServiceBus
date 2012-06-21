using SystemDot.Messaging.Channels.Messages.Distribution;

namespace SystemDot.Messaging.Channels.PubSub
{
    public interface ISubscriptionChannelBuilder 
    {
        IDistributionSubscriber Build(SubscriptionSchema toSchema);
    }
}