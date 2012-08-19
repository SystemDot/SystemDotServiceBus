using SystemDot.Messaging.Messages.Processing.RequestReply;

namespace SystemDot.Messaging.Messages.Processing.Filtering
{
    public class ReplyChannelMessageFilterStategy : IMessageFilterStrategy
    {
        readonly EndpointAddress recieverAddress;
        readonly ReplyAddressLookup replyAddressLookup;

        public ReplyChannelMessageFilterStategy(EndpointAddress recieverAddress, ReplyAddressLookup replyAddressLookup)
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