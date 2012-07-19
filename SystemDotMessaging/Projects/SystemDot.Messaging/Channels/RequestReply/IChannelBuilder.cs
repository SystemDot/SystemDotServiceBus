using SystemDot.Messaging.Channels.Publishing;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public interface IChannelBuilder 
    {
        void Build(SubscriptionSchema toSchema);
    }
}