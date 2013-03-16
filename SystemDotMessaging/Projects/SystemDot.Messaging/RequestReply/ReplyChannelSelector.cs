using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.RequestReply
{
    class ReplyChannelSelector : IMessageProcessor<MessagePayload, MessagePayload>
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
            this.addressLookup.SetCurrentRecieverAddress(toInput.GetToAddress());

            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}