namespace SystemDot.Messaging.Channels.Sending.Http
{
    public interface IMessageWebRequestor {
        void PutMessage(object message);
    }
}