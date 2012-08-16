using System;

namespace SystemDot.Messaging
{
    public interface IBus
    {
        event Action<object> MessageSent;
        event Action<object> MessagePublished;
        
        void Send(object message);
        void Reply(object message);
        void Publish(object message);
    }
}