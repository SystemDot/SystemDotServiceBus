using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Correlation
{
    class ReplyCorrelationApplier : MessageProcessor
    {
        readonly ReplyCorrelationLookup lookup;

        public ReplyCorrelationApplier(ReplyCorrelationLookup lookup)
        {
            Contract.Requires(lookup != null);
            this.lookup = lookup;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            SetCorrelationIdOnReplyFromRequest(toInput);
            OnMessageProcessed(toInput);
        }

        void SetCorrelationIdOnReplyFromRequest(MessagePayload toInput)
        {
            if (!lookup.HasCurrentCorrelationId()) return;
            toInput.SetCorrelationId(lookup.GetCurrentCorrelationId());
        }
    }
}