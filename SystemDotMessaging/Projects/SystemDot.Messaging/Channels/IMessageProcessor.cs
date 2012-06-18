using System;

namespace SystemDot.Messaging.Channels
{
    public interface IMessageProcessor<in TIn, out TOut>
    {
        void InputMessage(TIn toInput);
        event Action<TOut> MessageProcessed;
    }
}