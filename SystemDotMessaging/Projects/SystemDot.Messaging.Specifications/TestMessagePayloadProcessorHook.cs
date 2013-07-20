using System;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Specifications
{
    public class TestMessagePayloadProcessorHook : IMessageHook<MessagePayload>
    {
        public MessagePayload Message { get; private set; }

        public void ProcessMessage(MessagePayload toInput, Action<MessagePayload> toPerformOnOutput)
        {
            Message = toInput;
            toPerformOnOutput(toInput);
        }
    }
}