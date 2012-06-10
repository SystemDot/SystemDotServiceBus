namespace SystemDot.Messaging.Recieving
{
    public interface IConsume
    { 
    }

    public interface IConsume<in T> : IConsume
    {
        void Consume(T message);
    }
}