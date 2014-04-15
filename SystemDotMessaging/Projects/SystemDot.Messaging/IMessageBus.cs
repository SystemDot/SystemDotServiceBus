using System;
using System.Threading.Tasks;
using SystemDot.Messaging.Batching;

namespace SystemDot.Messaging
{
    public interface IMessageBus : IBus
    {
        event Action<object> MessageSent;
        event Action<object> MessageSentDirect;
        event Action<object> MessagePublished;
        event Action<object> MessageReplied;

        void SendDirect(object message);
        Task SendDirectAsync(object message);
        void SendDirect(object message, Action<Exception> onServerError);
        Task SendDirectAsync(object message, Action<Exception> onServerError);
        void SendDirect(object message, object handleReplyWith, Action<Exception> onServerError);
        Task SendDirectAsync(object message, object handleReplyWith, Action<Exception> onServerError);

        void Reply(object message);
        Batch BatchSend();
    }


}