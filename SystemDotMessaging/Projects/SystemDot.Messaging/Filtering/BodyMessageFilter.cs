using System;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Filtering
{
    public class BodyMessageFilter : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly EndpointAddress address;

        public BodyMessageFilter(EndpointAddress address)
        {
            this.address = address;
        }

        public void InputMessage(MessagePayload toInput)
        {
            if(!toInput.HasBody()) return;
            if(toInput.GetToAddress() != this.address) return;

            this.MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}