using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Direct;

namespace SystemDot.Messaging
{
    public class MessageBus : IBus
    {
        public event Action<object> MessageSent;
        public event Action<object> MessageSentDirect;
        public event Action<object> MessagePublished;
        public event Action<object> MessageReplied;

        public void Send(object message)
        {
            Contract.Requires(message != null);
            
            if (MessageSent == null) return;
            MessageSent(message);
        }

        public void SendDirect(object message)
        {
            Contract.Requires(message != null);
            
            if (MessageSentDirect == null) return;
            MessageSentDirect(message);
        }

        public void SendDirect(object message, Action<Exception> onServerError) 
        {
            Contract.Requires(message != null);
            Contract.Requires(onServerError != null);
            using (new DirectSendContext(onServerError))
            {
                SendDirect(message);
            }
        }

        public void SendDirect(object message, object handleReplyWith, Action<Exception> onServerError) 
        {
            Contract.Requires(message != null);
            Contract.Requires(handleReplyWith != null);
            Contract.Requires(onServerError != null);

            using (new DirectSendContext(onServerError, handleReplyWith))
            {
                SendDirect(message);
            }
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