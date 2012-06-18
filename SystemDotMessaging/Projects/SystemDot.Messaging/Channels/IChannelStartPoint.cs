using System;

namespace SystemDot.Messaging.Channels
{
    public interface IChannelStartPoint<T>
    {
        event Action<T> MessageProcessed;
    }
}