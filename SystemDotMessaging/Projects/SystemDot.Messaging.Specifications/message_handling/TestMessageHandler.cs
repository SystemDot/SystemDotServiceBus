using SystemDot.Messaging.Recieving;

namespace SystemDot.Messaging.Specifications.message_handling
{
    public class TestMessageHandler<T> : IMessageHandler<T>
    {
        public T HandledMessage { get; private set; }

        public void Handle(T message)
        {
            this.HandledMessage = message;
        }
    }
}