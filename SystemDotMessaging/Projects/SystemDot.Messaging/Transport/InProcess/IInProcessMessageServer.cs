using System.Collections.Generic;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.InProcess
{
    public interface IInProcessMessageServer
    {
        List<MessagePayload> InputMessage(MessagePayload toInput);
    }
}