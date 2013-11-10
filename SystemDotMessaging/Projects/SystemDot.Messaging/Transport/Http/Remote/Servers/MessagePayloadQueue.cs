using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Transport.Http.Remote.Servers
{
    class MessagePayloadQueue
    {
        readonly TimeSpan blockingTimeout;
        readonly Dictionary<MessageServer, UniqueBlockingQueue<MessagePayload>> queues;
        
        public MessagePayloadQueue(TimeSpan blockingTimeout)
        {
            this.blockingTimeout = blockingTimeout;
            queues = new Dictionary<MessageServer, UniqueBlockingQueue<MessagePayload>>();
        }

        public void Enqueue(MessagePayload toEnqueue)
        {
            Contract.Requires(toEnqueue != null);

            CreateQueueIfNonExistant(toEnqueue.GetToAddress().Server);

            queues[toEnqueue.GetToAddress().Server].Enqueue(toEnqueue);
        }

        public IEnumerable<MessagePayload> DequeueAll(MessageServer server)
        {
            Contract.Requires(server != null);
            
            CreateQueueIfNonExistant(server);
            
            return queues[server].DequeueAll();
        }

        void CreateQueueIfNonExistant(MessageServer server)
        {
            if (!queues.ContainsKey(server))
                queues[server] = new UniqueBlockingQueue<MessagePayload>(blockingTimeout);
        }
    }
}