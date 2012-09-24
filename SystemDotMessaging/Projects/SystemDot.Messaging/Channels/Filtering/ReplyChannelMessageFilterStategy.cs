using SystemDot.Messaging.Channels.RequestReply;

namespace SystemDot.Messaging.Channels.Filtering
{
    public class ReplyChannelMessageFilterStategy : IMessageFilterStrategy
    {
        readonly EndpointAddress recieverAddress;
        readonly ReplyAddressLookup replyAddressLookup;

        public ReplyChannelMessageFilterStategy(ReplyAddressLookup replyAddressLookup, EndpointAddress recieverAddress)
        {
            this.recieverAddress = recieverAddress;
            this.replyAddressLookup = replyAddressLookup;
        }

        public bool PassesThrough(object toCheck)
        {
            return this.recieverAddress == replyAddressLookup.GetCurrentRecieverAddress();
        }
    }
}