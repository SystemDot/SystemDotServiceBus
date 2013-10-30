using System.Diagnostics.Contracts;
using SystemDot.Messaging.Correlation;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.RequestReply
{
    class ReplyChannelSelector : MessageProcessor
    {
        readonly ReplyAddressLookup addressLookup;
        readonly ReplyCorrelationLookup correlationLookup;

        public ReplyChannelSelector(ReplyAddressLookup addressLookup, ReplyCorrelationLookup correlationLookup)
        {
            Contract.Requires(addressLookup != null);
            Contract.Requires(correlationLookup != null);

            this.addressLookup = addressLookup;
            this.correlationLookup = correlationLookup;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            SetCurentSenderAddress(toInput);
            SetCurrentReceiverAddress(toInput);
            SetCurrentCorrelationId(toInput);

            OnMessageProcessed(toInput);
        }

        void SetCurentSenderAddress(MessagePayload toInput)
        {
            this.addressLookup.SetCurrentSenderAddress(toInput.GetFromAddress());
        }

        void SetCurrentReceiverAddress(MessagePayload toInput)
        {
            this.addressLookup.SetCurrentRecieverAddress(toInput.GetToAddress());
        }

        void SetCurrentCorrelationId(MessagePayload toInput)
        {
            if (!toInput.HasCorrelationId()) return;
            correlationLookup.SetCurrentCorrelationId(toInput.GetCorrelationId());
        }
    }
}