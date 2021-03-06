using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using SystemDot.Logging;
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

            Logger.Debug("Sending message: {0}", message.GetType().Name);
            
            if (MessageSent == null) return;
            MessageSent(message);
        }

        public void SendDirect(object message)
        {
            Contract.Requires(message != null);

            Logger.Debug("Sending direct message: {0}", message.GetType().Name);
            
            using (new DirectSendContext())
            {
                OnMessageSentDirect(message);
            }
        }

        public Task SendDirectAsync(object message)
        {
            Contract.Requires(message != null);

            Logger.Debug("Sending direct message asynchronously: {0}", message.GetType().Name);
            
            return taskStarter.StartTask(() => SendDirect(message));
        }

        public void SendDirect(object message, Action<Exception> onServerError)
        {
            Contract.Requires(message != null);
            Contract.Requires(onServerError != null);

            Logger.Debug("Sending direct message: {0}", message.GetType().Name);
            
            using (new DirectSendContext(onServerError))
            {
                OnMessageSentDirect(message);
            }
        }

        public Task SendDirectAsync(object message, Action<Exception> onServerError)
        {
            Contract.Requires(message != null);
            Contract.Requires(onServerError != null);
            
            Logger.Debug("Sending direct message asynchronously: {0}", message.GetType().Name);
            return taskStarter.StartTask(() => SendDirect(message, onServerError));
        }

        public void SendDirect(object message, object handleReplyWith, Action<Exception> onServerError)
        {
            Contract.Requires(message != null);
            Contract.Requires(handleReplyWith != null);
            Contract.Requires(onServerError != null);
            
            Logger.Debug("Sending direct message: {0} with reply handler {1}", message.GetType().Name, handleReplyWith.GetType().Name);
            
            using (new DirectSendContext(onServerError, handleReplyWith))
            {
                OnMessageSentDirect(message);
            }
        }

        public Task SendDirectAsync(object message, object handleReplyWith, Action<Exception> onServerError)
        {
            Contract.Requires(message != null);
            Contract.Requires(handleReplyWith != null);
            Contract.Requires(onServerError != null);
            
            Logger.Debug("Sending direct message asynchronously: {0} with reply handler {1}", message.GetType().Name, handleReplyWith.GetType().Name);
            
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

            Logger.Debug("Replying with message: {0}", message.GetType().Name);
            
            if (MessageReplied == null) return;
            MessageReplied(message);
        }

        public void Publish(object message)
        {
            Contract.Requires(message != null);

            Logger.Debug("Publishing message: {0}", message.GetType().Name);
            
            if (MessagePublished == null) return;
            MessagePublished(message);
        }

        public Batch BatchSend()
        {
            Logger.Debug("Starting batch");

            return new Batch();
        }
    }
}