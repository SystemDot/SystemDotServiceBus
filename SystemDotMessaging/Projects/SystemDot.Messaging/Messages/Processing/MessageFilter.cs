using System;

namespace SystemDot.Messaging.Messages.Processing
{
    public class MessageFilter : IMessageProcessor<object, object>
    {
        public void InputMessage(object toInput)
        {
            MessageProcessed(toInput);
        }

        public event Action<object> MessageProcessed;
    }
}