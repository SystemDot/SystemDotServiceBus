using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class RequestRecieveChannelDistrbutionStrategy : IChannelDistrbutionStrategy<MessagePayload>
    {
        public void Distribute(ChannelDistributor<MessagePayload> distributor, MessagePayload toDistribute)
        {
            distributor.GetChannel(toDistribute.GetFromAddress()).InputMessage(toDistribute);
        }
    }
}