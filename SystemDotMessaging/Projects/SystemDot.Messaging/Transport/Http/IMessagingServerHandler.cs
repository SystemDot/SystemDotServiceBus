using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.Http
{
    [ContractClass(typeof(IMessagingServerHandlerContract))]
    interface IMessagingServerHandler
    {
        void HandleMessage(MessagePayload toHandle, List<MessagePayload> outgoingMessages);
    }

    [ContractClassFor(typeof(IMessagingServerHandler))]
    class IMessagingServerHandlerContract : IMessagingServerHandler
    {
        public void HandleMessage(MessagePayload toHandle, List<MessagePayload> outgoingMessages)
        { 
            Contract.Requires(toHandle != null);
            Contract.Requires(outgoingMessages != null);
        }
    }
}