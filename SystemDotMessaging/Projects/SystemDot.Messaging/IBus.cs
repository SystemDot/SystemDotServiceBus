using System;
using System.Collections.Generic;
using SystemDot.Messaging.Aggregation;
using SystemDot.Messaging.PointToPoint.Builders;

namespace SystemDot.Messaging
{
    public interface IBus
    {
        event Action<object> MessageSent;
        event Action<object> MessageSentLocal;
        event Action<object> MessagePublished;
        event Action<object> MessageReplied;

        void Send(object message);
        void SendLocal(object message);
        void Reply(object message);
        void Publish(object message);
        Aggregate Aggregate();
    }

    
}