using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.Remote.Clients;

namespace SystemDot.Messaging.Transport.Http.Remote.Servers
{
    class SentMessageHandler : IMessagingServerHandler
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

            LogMessage(toHandle);

            this.outgoingQueue.Enqueue(toHandle);
        }

        static void LogMessage(MessagePayload toHandle)
        {
            Logger.Debug("Handling sent message {0} for {1} from {2}",
                toHandle.Id,
                toHandle.GetToAddress().Channel,
                toHandle.GetToAddress().Server.Address);
        }
    }
}