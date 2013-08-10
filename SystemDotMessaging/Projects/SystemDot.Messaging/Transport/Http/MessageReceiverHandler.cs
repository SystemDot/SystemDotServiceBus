using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Direct;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Direct.Builders;

namespace SystemDot.Messaging.Transport.Http
{
    class MessageReceiverHandler : IMessagingServerHandler
    {
        readonly MessageReceiver messageReceiver;

        public MessageReceiverHandler(MessageReceiver messageReceiver)
        {
            Contract.Requires(messageReceiver != null);

            this.messageReceiver = messageReceiver;
        }

        public void HandleMessage(MessagePayload toHandle, List<MessagePayload> outgoingMessages)
        {
            if (toHandle.IsDirectChannelMessage()) return;

            messageReceiver.InputMessage(toHandle);
        }
    }
}