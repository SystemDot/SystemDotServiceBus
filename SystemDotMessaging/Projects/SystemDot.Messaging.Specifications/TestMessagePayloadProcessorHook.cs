using System;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Specifications
{
    public class TestMessagePayloadProcessorHook : IMessageProcessor<MessagePayload, MessagePayload>
    {
        public MessagePayload Message { get; private set; }

        public event Action<MessagePayload> MessageProcessed;

        public void InputMessage(MessagePayload toInput)
        {
            Message = toInput;
            MessageProcessed(toInput);
        }
    }
}