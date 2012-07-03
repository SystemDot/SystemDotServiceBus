namespace SystemDot.Messaging.Messages.Consuming
{
    public interface IMessageHandler
    { 
    }

    public interface IMessageHandler<in T> : IMessageHandler
    {
        void Handle(T message);
    }
}