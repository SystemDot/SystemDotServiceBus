using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;

namespace SystemDot.Messaging.Servers
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