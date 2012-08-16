using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Messages.Processing
{
    public class ReplyChannelSelector : IMessageProcessor<object, object>
    {
        readonly EndpointAddress replyAddress;
        readonly ReplyChannelLookup channelLookup;

        public ReplyChannelSelector(EndpointAddress replyAddress, ReplyChannelLookup channelLookup)
        {
            Contract.Requires(replyAddress != EndpointAddress.Empty);
            Contract.Requires(channelLookup != null);

            this.replyAddress = replyAddress;
            this.channelLookup = channelLookup;
        }

        public void InputMessage(object toInput)
        {
            Contract.Requires(toInput != null);

            this.channelLookup.SetCurrentChannel(replyAddress);
            MessageProcessed(toInput);
        }

        public event Action<object> MessageProcessed;
    }
}