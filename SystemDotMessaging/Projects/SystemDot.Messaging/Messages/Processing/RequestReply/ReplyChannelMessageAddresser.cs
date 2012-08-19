using System;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Messages.Processing.RequestReply
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