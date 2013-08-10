using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Direct
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
            if (!toHandle.IsDirectChannelMessage()) return;

            using (var context = new MessageReplyContext(toHandle.GetToAddress(), toHandle.GetFromAddress()))
            {
                messageReceiver.InputMessage(toHandle);
                outgoingMessages.AddRange(context.GetCurrentReplies());
            }
        }
    }
}