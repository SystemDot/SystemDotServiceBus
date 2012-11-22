using SystemDot.Messaging.Channels.Distribution;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class ReplySendChannelDistrbutionStrategy : IChannelDistrbutionStrategy<object>
    {
        readonly ReplyAddressLookup replyAddressLookup;

        public ReplySendChannelDistrbutionStrategy(ReplyAddressLookup replyAddressLookup)
        {
            this.replyAddressLookup = replyAddressLookup;
        }

        public void Distribute(ChannelDistributor<object> distributor, object toDistribute)
        {
            distributor.GetChannel(this.replyAddressLookup.GetCurrentSenderAddress()).InputMessage(toDistribute);
        }
    }
}