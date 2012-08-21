using System;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Messages.Processing
{
    public class BodyMessageHandler : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly EndpointAddress address;

        public BodyMessageHandler(EndpointAddress address)
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