using System;
using System.Collections.Generic;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport
{
    public interface IMessageTransporter
    {
        void TransportMessage(
            MessagePayload toTransport,
            Action<Exception> onException,
            Action onCompletion,
            Action<IEnumerable<MessagePayload>> onReceiveMessages);

        void TransportMessage(MessagePayload toTransport);
    }
}