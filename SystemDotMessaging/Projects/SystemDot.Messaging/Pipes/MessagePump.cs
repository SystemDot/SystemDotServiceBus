using System;
using SystemDot.Threading;

namespace SystemDot.Messaging.Pipes
{
    public class MessagePump : IPipe, IWorker
    {
        readonly BlockingQueue<object> queue;

        public event Action<object> MessagePublished;

        public MessagePump()
        {
            this.queue = new BlockingQueue<object>(new TimeSpan(0, 0, 1));
        }

        public void Publish(object message)
        {
            this.queue.Enqueue(message);
        }

        public void OnWorkStarted()
        {
        }

        public void PerformWork()
        {
            foreach (object toDistribute in this.queue.DequeueAll())
            {
                OnMessagePublished(toDistribute);
            }
        }

        public void OnWorkStopped()
        {
        }

        void OnMessagePublished(object message)
        {
            if(MessagePublished != null)
                MessagePublished(message);
        }
    }
}