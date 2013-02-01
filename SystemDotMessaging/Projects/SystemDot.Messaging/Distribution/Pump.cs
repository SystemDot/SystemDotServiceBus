using System;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Distribution
{
    public class Pump<T> : IMessageProcessor<T, T>
    {
        readonly ITaskStarter taskStarter;

        public event Action<T> MessageProcessed;

        public Pump(ITaskStarter taskStarter)
        {
            this.taskStarter = taskStarter;
        }

        public void InputMessage(T toInput)
        {
            this.taskStarter.StartTask(() => OnItemPushed(toInput));
        }

        void OnItemPushed(T message)
        {
            if (this.MessageProcessed != null)
                this.MessageProcessed(message);
        }
    }
}