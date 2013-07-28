using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.RequestReply
{
    class ReplyChannelMessageFilterStategy : IMessageFilterStrategy
    {
        readonly EndpointAddress receiverAddress;
        readonly ReplyAddressLookup replyAddressLookup;

        public ReplyChannelMessageFilterStategy(ReplyAddressLookup replyAddressLookup, EndpointAddress receiverAddress)
        {
            this.receiverAddress = receiverAddress;
            this.replyAddressLookup = replyAddressLookup;
        }

        public bool PassesThrough(object toCheck)
        {
            return receiverAddress.Channel == replyAddressLookup.GetCurrentRecieverAddress().Channel;
        }
    }
}