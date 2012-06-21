using System;

namespace SystemDot.Messaging.Channels.Messages.Processing
{
    public interface IMessageProcessor<in TIn, out TOut>
    {
        void InputMessage(TIn toInput);
        event Action<TOut> MessageProcessed;
    }
}