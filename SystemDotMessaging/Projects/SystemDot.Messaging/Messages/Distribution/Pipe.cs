using System;

namespace SystemDot.Messaging.Messages.Distribution
{
    public class Pipe<T> : IMessageProcessor<T, T>
    {
        public void InputMessage(T message)
        {
            MessageProcessed(message);
        }

        public event Action<T> MessageProcessed;
    }
}