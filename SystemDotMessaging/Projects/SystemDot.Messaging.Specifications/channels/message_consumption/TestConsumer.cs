namespace SystemDot.Messaging.Specifications.channels.message_consumption
{
    public class TestConsumer<T> : IConsume<T>
    {
        public T ConsumedMessage { get; private set; }

        public void Consume(T message)
        {
            this.ConsumedMessage = message;
        }
    }
}