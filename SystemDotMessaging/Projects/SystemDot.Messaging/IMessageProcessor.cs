using System;

namespace SystemDot.Messaging
{
    public interface IMessageProcessor<in TIn, out TOut>
    {
        void InputMessage(TIn toInput);
        event Action<TOut> MessageProcessed;
    }
}