using System.Collections.Generic;

namespace SystemDot.Messaging.Specifications
{
    public class TestMessageHandler<T>
    {
        public T LastHandledMessage { get; private set; }

        public List<T> HandledMessages { get; private set; }

        public TestMessageHandler()
        {
            HandledMessages = new List<T>();
        }

        public virtual void Handle(T message)
        {
            LastHandledMessage = message;
            HandledMessages.Add(message);
        }
    }
}