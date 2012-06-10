using System;

namespace SystemDot.Messaging.Pipes
{
    public interface IPipe 
    {
        event Action<object> MessagePublished;
        void Publish(object message);
    }
}