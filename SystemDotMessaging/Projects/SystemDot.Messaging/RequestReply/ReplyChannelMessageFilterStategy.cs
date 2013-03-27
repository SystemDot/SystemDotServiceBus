using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.RequestReply
{
    class ReplyChannelMessageFilterStategy : IMessageFilterStrategy
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