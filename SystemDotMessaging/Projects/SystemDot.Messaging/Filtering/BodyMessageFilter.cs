using System;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Filtering
{
    class BodyMessageFilter : MessageProcessor
    {
        readonly EndpointAddress address;

        public BodyMessageFilter(EndpointAddress address)
        {
            this.address = address;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            if(!toInput.HasBody()) return;
            if(toInput.GetToAddress() != address) return;

            OnMessageProcessed(toInput);
        }
    }
}