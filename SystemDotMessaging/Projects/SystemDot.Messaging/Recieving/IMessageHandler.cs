namespace SystemDot.Messaging.Recieving
{
    public interface IMessageHandler
    { 
    }

    public interface IMessageHandler<in T> : IMessageHandler
    {
        void Handle(T message);
    }
}