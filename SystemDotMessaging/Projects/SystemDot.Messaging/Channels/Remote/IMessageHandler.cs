namespace SystemDot.Messaging.Channels.Remote
{
    public interface IMessageHandler
    { 
    }

    public interface IMessageHandler<in T> : IMessageHandler
    {
        void Handle(T message);
    }
}