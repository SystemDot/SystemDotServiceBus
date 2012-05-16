using System;

namespace SystemDot.Messaging.Threading
{
    public class ThreadedDistributor : IDistributor
    {
        readonly int workerThreads;
        readonly IThreader threader;
        readonly BlockingQueue<object> queue;
        bool isRunning;
        
        public ThreadedDistributor(int workerThreads, IThreader threader)
        {
            this.workerThreads = workerThreads;
            this.threader = threader;
            this.queue = new BlockingQueue<object>(new TimeSpan(0, 0, 1));
        }

        void Perform(Action<object> toDistributeTo)
        {
            do
            {
                foreach (object toDistribute in this.queue.DequeueAll())
                {
                    toDistributeTo(toDistribute);
                }
            } 
            while (this.isRunning);
        }

        public void Start(Action<object> toDistributeTo)
        {
            for (int i = 0; i < this.workerThreads; i++)
            {
                this.threader.Start(() => Perform(toDistributeTo));
            }
            this.isRunning = true;
        }

        public void Distribute(object toDistribute)
        {
            this.queue.Enqueue(toDistribute);
        }

        public void Stop()
        {
            this.isRunning = false;
        }
    }
}