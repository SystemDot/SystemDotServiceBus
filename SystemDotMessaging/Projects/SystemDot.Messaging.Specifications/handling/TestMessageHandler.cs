using SystemDot.Messaging.Channels.Remote;

namespace SystemDot.Messaging.Specifications.handling
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