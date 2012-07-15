using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging
{
    public interface IBus : IMessageProcessor<object>
    {
        void Send(object message);
    }
}