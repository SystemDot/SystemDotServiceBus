using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;

namespace SystemDot.Messaging.Servers
{
    public class SentMessageHandler : IMessagingServerHandler
    {
        readonly MessagePayloadQueue outgoingQueue;

        public SentMessageHandler(MessagePayloadQueue outgoingQueue)
        {
            Contract.Requires(outgoingQueue != null);
            this.outgoingQueue = outgoingQueue;
        }

        public void HandleMessage(MessagePayload toHandle, List<MessagePayload> outgoingMessages)
        {
            if (!toHandle.HasBody()) 
                return;

            this.outgoingQueue.Enqueue(toHandle);
        }
    }
}