using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Messages.Processing
{
    public class ReplyChannelSelector : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly ReplyChannelLookup channelLookup;

        public ReplyChannelSelector(ReplyChannelLookup channelLookup)
        {
            Contract.Requires(channelLookup != null);

            this.channelLookup = channelLookup;
        }

        public void InputMessage(MessagePayload toInput)
        {
            Contract.Requires(toInput != null);

            this.channelLookup.SetCurrentChannel(toInput.GetFromAddress());
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}