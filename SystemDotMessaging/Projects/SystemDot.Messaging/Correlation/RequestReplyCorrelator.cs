using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Correlation
{
    class RequestReplyCorrelator : MessageProcessor
    {
        readonly CorrelationLookup lookup;

        public RequestReplyCorrelator(CorrelationLookup lookup)
        {
            Contract.Requires(lookup != null);
            this.lookup = lookup;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            if (toInput.HasCorrelationId() && !TryCorrelate(toInput)) return;
            OnMessageProcessed(toInput);
        }

        bool TryCorrelate(MessagePayload toInput)
        {
            return this.lookup.TryCorrelate(toInput.GetCorrelationId());
        }
    }
}