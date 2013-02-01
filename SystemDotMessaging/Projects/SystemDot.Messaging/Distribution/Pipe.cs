using System;

namespace SystemDot.Messaging.Distribution
{
    public class Pipe<T> : IMessageProcessor<T, T>
    {
        public void InputMessage(T message)
        {
            this.MessageProcessed(message);
        }

        public event Action<T> MessageProcessed;
    }
}