using System;
using SystemDot.Messaging.Batching;

namespace SystemDot.Messaging
{
    public interface IBus
    {
        event Action<object> MessageSent;
        event Action<object> MessageSentDirect;
        event Action<object> MessagePublished;
        event Action<object> MessageReplied;

        void Send(object message);
        void SendDirect(object message);
        void SendDirect(object message, Action<Exception> onServerError);
        void SendDirect(object message, object handleReplyWith, Action<Exception> onServerError);
        void Reply(object message);
        void Publish(object message);
        Batch BatchSend();
    }

    
}