using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Transport.Http.Remote.Servers
{
    public class MessagePayloadQueue
    {
        readonly TimeSpan blockingTimeout;
        readonly Dictionary<ServerPath, BlockingQueue<MessagePayload>> queues;
        
        public MessagePayloadQueue(TimeSpan blockingTimeout)
        {
            this.blockingTimeout = blockingTimeout;
            this.queues = new Dictionary<ServerPath, BlockingQueue<MessagePayload>>();
        }

        public void Enqueue(MessagePayload toEnqueue)
        {
            Contract.Requires(toEnqueue != null);

            CreateQueueIfNonExistant(toEnqueue.GetToAddress().ServerPath);

            this.queues[toEnqueue.GetToAddress().ServerPath].Enqueue(toEnqueue);
        }

        public IEnumerable<MessagePayload> DequeueAll(ServerPath serverPath)
        {
            Contract.Requires(serverPath != null);
            
            CreateQueueIfNonExistant(serverPath);
            
            return this.queues[serverPath].DequeueAll();
        }

        void CreateQueueIfNonExistant(ServerPath serverPath)
        {
            if (!this.queues.ContainsKey(serverPath))
                this.queues[serverPath] = new BlockingQueue<MessagePayload>(this.blockingTimeout);
        }
    }
}