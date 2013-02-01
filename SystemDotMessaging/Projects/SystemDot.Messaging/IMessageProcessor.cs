using System;

namespace SystemDot.Messaging
{
    public interface IMessageProcessor<in TIn, TOut> : IMessageInputter<TIn>, IMessageProcessor<TOut>
    {
    }

    public interface IMessageProcessor<T>
    {
        event Action<T> MessageProcessed;
    }

    public interface IMessageProcessor : IMessageProcessor<object>
    {
    }
}