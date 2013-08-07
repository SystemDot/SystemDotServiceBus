using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.Http
{
    class MessageReceiverHandler : IMessagingServerHandler
    {
        readonly IMessageReceiver messageReceiver;

        public MessageReceiverHandler(IMessageReceiver messageReceiver)
        {
            Contract.Requires(messageReceiver != null);

            this.messageReceiver = messageReceiver;
        }

        public void HandleMessage(MessagePayload toHandle, List<MessagePayload> outgoingMessages)
        {
            messageReceiver.InputMessage(toHandle);
        }
    }
}