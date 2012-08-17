using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging
{
    public class MessageBus : IBus
    {
        public event Action<object> MessageSent;
        public event Action<object> MessagePublished;
        public event Action<object> MessageReplied;

        public void Send(object message)
        {
            Contract.Requires(message != null);
            MessageSent(message);
        }

        public void Reply(object message)
        {
            Contract.Requires(message != null);
            MessageReplied(message);
        }

        public void Publish(object message)
        {
            Contract.Requires(message != null);
            MessagePublished(message);
        }
    }
}