namespace SystemDot.Messaging.Specifications
{
    public class TestMessageHandler<T>
    {
        public T HandledMessage { get; private set; }

        public virtual void Handle(T message)
        {
            HandledMessage = message;
        }
    }
}