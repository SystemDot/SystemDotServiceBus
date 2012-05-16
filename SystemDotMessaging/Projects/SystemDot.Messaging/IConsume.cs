namespace SystemDot.Messaging
{
    public interface IConsume
    { 
    }

    public interface IConsume<in T> : IConsume
    {
        void Consume(T message);
    }
}