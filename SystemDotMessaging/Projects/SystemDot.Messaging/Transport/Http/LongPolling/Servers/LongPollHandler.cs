using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Transport.Http.LongPolling.Servers
{
    public class LongPollHandler : IMessagingServerHandler
    {
        readonly MessagePayloadQueue outgoingQueue;

        public LongPollHandler(MessagePayloadQueue outgoingQueue)
        {
            Contract.Requires(outgoingQueue != null);
            this.outgoingQueue = outgoingQueue;
        }

        public void HandleMessage(MessagePayload toHandle, List<MessagePayload> outgoingMessages)
        {
            if (!toHandle.IsLongPollRequest())
                return;

            outgoingMessages.AddRange(this.outgoingQueue.DequeueAll(toHandle.GetToAddress()));
        }
    }
}