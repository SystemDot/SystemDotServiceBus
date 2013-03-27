using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Batching;

namespace SystemDot.Messaging
{
    public class MessageBus : IBus
    {
        public event Action<object> MessageSent;
        public event Action<object> MessageSentLocal;
        public event Action<object> MessagePublished;
        public event Action<object> MessageReplied;

        public void Send(object message)
        {
            Contract.Requires(message != null);
            
            if (MessageSent == null) return;
            MessageSent(message);
        }

        public void SendLocal(object message)
        {
            Contract.Requires(message != null);

            if (MessageSentLocal == null) return;
            MessageSentLocal(message);
        }

        public void Reply(object message)
        {
            Contract.Requires(message != null);

            if (MessageReplied == null) return;
            MessageReplied(message);
        }

        public void Publish(object message)
        {
            Contract.Requires(message != null);

            if (MessagePublished == null) return;
            MessagePublished(message);
        }

        public Batch BatchSend()
        {
            return new Batch();
        }
    }
}