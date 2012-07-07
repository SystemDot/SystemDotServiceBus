using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.Messages.Packaging;

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

            toHandle.GetLongPollRequestAddresses()
                .ForEach(a => 
                    outgoingMessages.AddRange(this.outgoingQueue.DequeueAll(a)));
        }
    }
}