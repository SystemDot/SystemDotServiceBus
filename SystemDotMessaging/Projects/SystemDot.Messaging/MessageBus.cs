using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Direct;
using SystemDot.Parallelism;

namespace SystemDot.Messaging
{
    public class MessageBus : IBus
    {
        readonly ITaskStarter taskStarter;

        public MessageBus(ITaskStarter taskStarter)
        {
            Contract.Requires(taskStarter != null);
            this.taskStarter = taskStarter;
        }

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

            using (new DirectSendContext())
            {
                OnMessageSentDirect(message);
            }
        }

        public Task SendDirectAsync(object message)
        {
            Contract.Requires(message != null);
            return taskStarter.StartTask(() => SendDirect(message));
        }

        public void SendDirect(object message, Action<Exception> onServerError)
        {
            Contract.Requires(message != null);
            Contract.Requires(onServerError != null);

            using (new DirectSendContext(onServerError))
            {
                OnMessageSentDirect(message);
            }
        }

        public Task SendDirectAsync(object message, Action<Exception> onServerError)
        {
            return taskStarter.StartTask(() => SendDirect(message, onServerError));
        }

        public void SendDirect(object message, object handleReplyWith, Action<Exception> onServerError)
        {
            Contract.Requires(message != null);
            Contract.Requires(handleReplyWith != null);
            Contract.Requires(onServerError != null);

            using (new DirectSendContext(onServerError, handleReplyWith))
            {
                OnMessageSentDirect(message);
            }
        }

        public Task SendDirectAsync(object message, object handleReplyWith, Action<Exception> onServerError)
        {
            return taskStarter.StartTask(() => SendDirect(message, handleReplyWith, onServerError));
        }

        void OnMessageSentDirect(object message)
        {
            if (MessageSentDirect == null) return;
            MessageSentDirect(message);
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