using System;
using System.Collections.Concurrent;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Distribution
{
    public class Queue<T> : IMessageProcessor<T, T>
    {
        readonly ConcurrentQueue<T> queue;
        readonly ITaskStarter taskStarter;
        readonly object locker;
        bool isDequeueing;

        public Queue(ITaskStarter taskStarter)
        {
            this.taskStarter = taskStarter;
            this.queue = new ConcurrentQueue<T>();
            this.locker = new object();
        }

        public void InputMessage(T message)
        {
            this.queue.Enqueue(message);

            ScheduleDequeuing();
        }

        void ScheduleDequeuing()
        {
            lock (this.locker)
            {
                if (!this.isDequeueing)
                {
                    this.isDequeueing = true;
                    this.taskStarter.StartTask(Dequeue);
                }
            }
        }

        void Dequeue()
        {
            T message;

            lock (this.locker)
            {
                this.isDequeueing = this.queue.TryDequeue(out message);
            }

            if (this.isDequeueing)
            {

                OnItemPushed(message);
                Dequeue();
            }
        }

        void OnItemPushed(T message)
        {
            if (this.MessageProcessed != null)
                this.MessageProcessed(message);
        }

        public event Action<T> MessageProcessed;
    }
}