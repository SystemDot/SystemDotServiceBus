using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
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

            toHandle.GetLongPollRequestAddresses().ForEach(a =>
            {
                Logger.Info("Handling long pole request for {0}", a);
                outgoingMessages.AddRange(this.outgoingQueue.DequeueAll(a));
                Logger.Info("Handled long pole request for {0}", a);
            });
        }
    }
}