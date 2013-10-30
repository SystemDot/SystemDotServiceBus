using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Correlation
{
    class CorrelationAssigner : MessageProcessor
    {
        readonly CorrelationLookup lookup;

        public CorrelationAssigner(CorrelationLookup lookup)
        {
            Contract.Requires(lookup != null);
            this.lookup = lookup;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            SetNewCorrelationId(toInput);
            RegisterCorrelationInLookup(toInput);
            OnMessageProcessed(toInput);
        }
        
        void SetNewCorrelationId(MessagePayload toInput)
        {
            toInput.SetCorrelationId(Guid.NewGuid());
        }

        void RegisterCorrelationInLookup(MessagePayload toInput)
        {
            lookup.RegisterCorrelationStarted(toInput.GetCorrelationId());
        }
    }
}