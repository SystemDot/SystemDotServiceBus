using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Transport.Http.LongPolling.Servers
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
            if (toHandle.IsLongPollRequest()) 
                return;
            
            this.outgoingQueue.Enqueue(toHandle);
        }
    }
}