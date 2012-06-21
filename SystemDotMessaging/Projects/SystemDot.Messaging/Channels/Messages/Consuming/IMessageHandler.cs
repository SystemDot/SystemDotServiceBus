namespace SystemDot.Messaging.Channels.Messages.Consuming
{
    public interface IMessageHandler
    { 
    }

    public interface IMessageHandler<in T> : IMessageHandler
    {
        void Handle(T message);
    }
}