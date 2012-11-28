using System;
using System.Collections.Concurrent;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Channels.Distribution
{
    public class Queue<T> : IMessageProcessor<T, T>
    {
        readonly ConcurrentQueue<T> queue;
        readonly ITaskStarter taskStarter;

        public Queue(ITaskStarter taskStarter)
        {
            this.taskStarter = taskStarter;
            this.queue = new ConcurrentQueue<T>();
        }

        public void InputMessage(T message)
        {
            this.queue.Enqueue(message);
        }

        public void Start()
        {
            this.taskStarter.StartTask(Dequeue);
        }

        void Dequeue()
        {
            T message;

            while(this.queue.TryDequeue(out message))
                OnItemPushed(message);

            Start();
        }

        void OnItemPushed(T message)
        {
            if (MessageProcessed != null)
                MessageProcessed(message);
        }

        public event Action<T> MessageProcessed;
    }
}