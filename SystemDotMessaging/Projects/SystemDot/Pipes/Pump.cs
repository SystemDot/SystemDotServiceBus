using System;
using SystemDot.Threading;

namespace SystemDot.Pipes
{
    public class Pump<T> : IPipe<T>
    {
        readonly IThreadPool threadPool;
        
        public event Action<T> ItemPushed;

        public Pump(IThreadPool threadPool)
        {
            this.threadPool = threadPool;
        }

        public void Push(T item)
        {
            this.threadPool.QueueTask(() => OnItemPushed(item));
        }

        void OnItemPushed(T message)
        {
            if (ItemPushed != null)
                ItemPushed(message);
        }
    }
}