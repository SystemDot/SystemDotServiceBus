using System;
using SystemDot.Messaging.Channels.Messages.Processing;
using SystemDot.Threading;

namespace SystemDot.Messaging.Channels.Messages.Distribution
{
    public class Pump<T> : IMessageProcessor<T, T>
    {
        readonly IThreadPool threadPool;

        public event Action<T> MessageProcessed;

        public Pump(IThreadPool threadPool)
        {
            this.threadPool = threadPool;
        }

        public void InputMessage(T toInput)
        {
            this.threadPool.QueueTask(() => OnItemPushed(toInput));
        }

        void OnItemPushed(T message)
        {
            if (MessageProcessed != null)
                MessageProcessed(message);
        }
    }
}