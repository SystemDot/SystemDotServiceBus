using System;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class ReplyChannelMessageAddresser : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly ReplyAddressLookup addressLookup;
        readonly EndpointAddress address;

        public ReplyChannelMessageAddresser(ReplyAddressLookup addressLookup, EndpointAddress address)
        {
            this.addressLookup = addressLookup;
            this.address = address;
        }

        public void InputMessage(MessagePayload toInput)
        {
            toInput.SetFromAddress(address);
            toInput.SetToAddress(this.addressLookup.GetCurrentSenderAddress());
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}