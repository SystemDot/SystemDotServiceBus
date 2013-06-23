using System;

namespace SystemDot.Messaging.Specifications.hooks
{
    public class TestMessageProcessorHook : IMessageProcessor<object, object>
    {
        public object Message { get; private set; }

        public event Action<object> MessageProcessed;

        public void InputMessage(object toInput)
        {
            this.Message = toInput;
            this.MessageProcessed(toInput);
        }
    }
}