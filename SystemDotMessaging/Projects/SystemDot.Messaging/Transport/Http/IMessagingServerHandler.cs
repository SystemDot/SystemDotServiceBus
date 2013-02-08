using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.Http
{
    [ContractClass(typeof(IMessagingServerHandlerContract))]
    public interface IMessagingServerHandler
    {
        void HandleMessage(MessagePayload toHandle, List<MessagePayload> outgoingMessages);
    }

    [ContractClassFor(typeof(IMessagingServerHandler))]
    public class IMessagingServerHandlerContract : IMessagingServerHandler
    {
        public void HandleMessage(MessagePayload toHandle, List<MessagePayload> outgoingMessages)
        { 
            Contract.Requires(toHandle != null);
            Contract.Requires(outgoingMessages != null);
        }
    }
}