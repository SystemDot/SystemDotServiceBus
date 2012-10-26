using System;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;

namespace SystemDot.Messaging.Channels.Filtering
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

            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}