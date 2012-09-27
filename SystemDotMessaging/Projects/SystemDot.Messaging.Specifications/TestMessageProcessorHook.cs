using System;
using SystemDot.Messaging.Channels;

namespace SystemDot.Messaging.Specifications
{
    public class TestMessageProcessorHook : IMessageProcessor<object, object>
    {
        public object Message { get; private set; }

        public event Action<object> MessageProcessed;

        public void InputMessage(object toInput)
        {
            Message = toInput;
            MessageProcessed(toInput);
        }
    }
}