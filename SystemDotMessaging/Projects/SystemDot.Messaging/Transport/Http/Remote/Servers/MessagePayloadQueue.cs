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
        readonly Dictionary<EndpointAddress, BlockingQueue<MessagePayload>> queues;
        
        public MessagePayloadQueue(TimeSpan blockingTimeout)
        {
            this.blockingTimeout = blockingTimeout;
            this.queues = new Dictionary<EndpointAddress, BlockingQueue<MessagePayload>>();
        }

        public void Enqueue(MessagePayload toEnqueue)
        {
            Contract.Requires(toEnqueue != null);

            CreateQueueIfNonExistant(toEnqueue.GetToAddress());

            this.queues[toEnqueue.GetToAddress()].Enqueue(toEnqueue);
        }

        public IEnumerable<MessagePayload> DequeueAll(EndpointAddress address)
        {
            Contract.Requires(address != EndpointAddress.Empty);
            
            CreateQueueIfNonExistant(address);
            
            return this.queues[address].DequeueAll();
        }

        void CreateQueueIfNonExistant(EndpointAddress address)
        {
            if (!this.queues.ContainsKey(address))
                this.queues[address] = new BlockingQueue<MessagePayload>(this.blockingTimeout);
        }
    }
}