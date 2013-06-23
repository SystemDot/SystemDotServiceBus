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
        readonly Dictionary<ServerRoute, BlockingQueue<MessagePayload>> queues;
        
        public MessagePayloadQueue(TimeSpan blockingTimeout)
        {
            this.blockingTimeout = blockingTimeout;
            this.queues = new Dictionary<ServerRoute, BlockingQueue<MessagePayload>>();
        }

        public void Enqueue(MessagePayload toEnqueue)
        {
            Contract.Requires(toEnqueue != null);

            CreateQueueIfNonExistant(toEnqueue.GetToAddress().Route);

            this.queues[toEnqueue.GetToAddress().Route].Enqueue(toEnqueue);
        }

        public IEnumerable<MessagePayload> DequeueAll(ServerRoute serverRoute)
        {
            Contract.Requires(serverRoute != null);
            
            CreateQueueIfNonExistant(serverRoute);
            
            return this.queues[serverRoute].DequeueAll();
        }

        void CreateQueueIfNonExistant(ServerRoute serverRoute)
        {
            if (!this.queues.ContainsKey(serverRoute))
                this.queues[serverRoute] = new BlockingQueue<MessagePayload>(this.blockingTimeout);
        }
    }
}