using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class ReplyChannelSelector : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly ReplyAddressLookup addressLookup;

        public ReplyChannelSelector(ReplyAddressLookup addressLookup)
        {
            Contract.Requires(addressLookup != null);

            this.addressLookup = addressLookup;
        }

        public void InputMessage(MessagePayload toInput)
        {
            Contract.Requires(toInput != null);

            this.addressLookup.SetCurrentSenderAddress(toInput.GetFromAddress());
            
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}