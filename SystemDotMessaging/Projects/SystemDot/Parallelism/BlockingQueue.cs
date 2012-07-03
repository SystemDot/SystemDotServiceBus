using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;

namespace SystemDot.Parallelism
{
    public class BlockingQueue<T>
    {
        readonly Queue<T> queue;
        readonly object locker;
        readonly TimeSpan blockingTimeout;

        public BlockingQueue(TimeSpan blockingTimeout)
        {
            Contract.Requires(blockingTimeout != null);
            
            this.blockingTimeout = blockingTimeout;
            this.queue = new Queue<T>();
            this.locker = new object();
        }

        public void Enqueue(T toEnqueue)
        {
            Contract.Requires(!toEnqueue.Equals(default(T)));
            
            lock (this.locker)
            {
                this.queue.Enqueue(toEnqueue);

                Monitor.Pulse(this.locker);
            }
        }

        public IEnumerable<T> DequeueAll()
        {
            lock (this.locker)
            {
                if (queue.Count == 0)
                    Monitor.Wait(this.locker, blockingTimeout);

                var output = new List<T>();
                
                while (queue.Count > 0)
                    output.Add(this.queue.Dequeue());

                return output;
            }
        }
    }
}