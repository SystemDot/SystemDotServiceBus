using System;

namespace SystemDot.Messaging.Channels.Messages
{
    public interface IMessageProcessor<in TIn, TOut> : IMessageInputter<TIn>, IMessageProcessor<TOut>
    {
    }

    public interface IMessageProcessor<T>
    {
        event Action<T> MessageProcessed;
    }
}