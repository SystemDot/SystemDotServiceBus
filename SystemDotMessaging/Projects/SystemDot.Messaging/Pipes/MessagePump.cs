using System;
using SystemDot.Threading;

namespace SystemDot.Messaging.Pipes
{
    public class MessagePump : IPipe
    {
        readonly IThreadPool threadPool;
        
        public event Action<object> MessagePublished;

        public MessagePump(IThreadPool threadPool)
        {
            this.threadPool = threadPool;
        }

        public void Publish(object message)
        {
            this.threadPool.QueueTask(() => OnMessagePublished(message));
        }

        void OnMessagePublished(object message)
        {
            if(MessagePublished != null)
                MessagePublished(message);
        }
    }
}