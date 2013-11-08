using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;

namespace SystemDot.Parallelism
{
    public class UniqueBlockingQueue<T>
    {
        readonly Queue<T> queue;
        readonly object locker;
        readonly TimeSpan blockingTimeout;

        public UniqueBlockingQueue(TimeSpan blockingTimeout)
        {
            Contract.Requires(blockingTimeout != null);
            
            this.blockingTimeout = blockingTimeout;
            this.queue = new Queue<T>();
            this.locker = new object();
        }

        public void Enqueue(T toEnqueue)
        {
            Contract.Requires(!toEnqueue.Equals(default(T)));
            
            lock (locker)
            {
                if (queue.Contains(toEnqueue)) return;

                queue.Enqueue(toEnqueue);

                Monitor.Pulse(locker);
            }
        }

        public IEnumerable<T> DequeueAll()
        {
            lock (locker)
            {
                if (queue.Count == 0)
                    Monitor.Wait(locker, blockingTimeout);

                var output = new List<T>();
                
                while (queue.Count > 0)
                    output.Add(queue.Dequeue());

                return output;
            }
        }
    }
}