using System;
using SystemDot.Messaging.Hooks;

namespace SystemDot.Messaging.Specifications
{
    public class TestMessageProcessorHook : IMessageHook<object>
    {
        public object Message { get; private set; }

        public void ProcessMessage(object toInput, Action<object> toPerformOnOutput)
        {
            Message = toInput;
            toPerformOnOutput(toInput);
        }
    }
}