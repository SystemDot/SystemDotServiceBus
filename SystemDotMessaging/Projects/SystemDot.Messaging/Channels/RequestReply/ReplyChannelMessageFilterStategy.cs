using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Filtering;

namespace SystemDot.Messaging.Channels.RequestReply
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
            return this.recieverAddress == this.replyAddressLookup.GetCurrentRecieverAddress();
        }
    }
}