using System;
using System.Collections.Concurrent;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Distribution
{
    public class Queue<T> : IMessageProcessor<T, T>
    {
        readonly BlockingCollection<T> producedMessages = new BlockingCollection<T>();

        public Queue(ITaskStarter taskStarter)
        {
            taskStarter.StartTask(ConsumeMessages);    
        }

        public event Action<T> MessageProcessed;

        void OnItemPushed(T message)
        {
            if (MessageProcessed != null) MessageProcessed(message);
        }

        public void InputMessage(T toInput)
        {
            this.producedMessages.Add(toInput);
        }

        void ConsumeMessages()
        {
            foreach (var message in this.producedMessages.GetConsumingEnumerable())
            {
                OnItemPushed(message);
            }
        }
    }
}