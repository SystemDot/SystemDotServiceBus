using System.Collections.Generic;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Servers
{
    public interface IMessagingServerHandler
    {
        void HandleMessage(MessagePayload toHandle);
        IEnumerable<MessagePayload> Reply();
        bool ShouldHandleMessage(MessagePayload toCheck);
    }
}