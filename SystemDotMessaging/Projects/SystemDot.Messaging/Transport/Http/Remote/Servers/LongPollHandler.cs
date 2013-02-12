using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Transport.Http.Remote.Clients;

namespace SystemDot.Messaging.Transport.Http.Remote.Servers
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

            ServerPath serverPath = toHandle.GetLongPollRequestServerPath();

            Logger.Debug("Handling long pole request for {0}", serverPath);
            outgoingMessages.AddRange(this.outgoingQueue.DequeueAll(serverPath));
            Logger.Debug("Handled long pole request for {0}", serverPath);
        }
    }
}