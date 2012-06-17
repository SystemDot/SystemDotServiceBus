using System;

namespace SystemDot.Messaging
{
    public interface IMessageStartPoint<T>
    {
        event Action<T> MessageProcessed;
    }
}