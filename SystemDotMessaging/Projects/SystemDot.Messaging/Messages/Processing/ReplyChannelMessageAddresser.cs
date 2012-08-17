using System;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Messages.Processing
{
    public class ReplyChannelMessageAddresser : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly ReplyChannelLookup channelLookup;
        readonly EndpointAddress address;

        public ReplyChannelMessageAddresser(ReplyChannelLookup channelLookup, EndpointAddress address)
        {
            this.channelLookup = channelLookup;
            this.address = address;
        }

        public void InputMessage(MessagePayload toInput)
        {
            toInput.SetFromAddress(address);
            toInput.SetToAddress(this.channelLookup.GetCurrentChannel());
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}