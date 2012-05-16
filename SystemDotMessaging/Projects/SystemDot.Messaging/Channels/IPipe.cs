using System;

namespace SystemDot.Messaging.Channels
{
    public interface IPipe 
    {
        event Action<object> MessagePublished;
        void Publish(object message);
    }
}