using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Threading;

namespace SystemDot.Messaging.Servers
{
    public class MessagePayloadQueue
    {
        readonly Dictionary<string, BlockingQueue<MessagePayload>> queues;
        readonly TimeSpan blockingTimeout;

        public MessagePayloadQueue(TimeSpan blockingTimeout)
        {
            Contract.Requires(blockingTimeout != null);

            this.blockingTimeout = blockingTimeout;
            this.queues = new Dictionary<string, BlockingQueue<MessagePayload>>();
        }

        public void Enqueue(MessagePayload toEnqueue)
        {
            Contract.Requires(toEnqueue != null);

            CreateQueueIfNonExistant(toEnqueue.Address);

            this.queues[toEnqueue.Address].Enqueue(toEnqueue);
        }

        public IEnumerable<MessagePayload> DequeueAll(string address)
        {
            CreateQueueIfNonExistant(address);
            
            return this.queues[address].DequeueAll();
        }

        void CreateQueueIfNonExistant(string address)
        {
            Contract.Requires(!string.IsNullOrEmpty(address));
            
            if (!this.queues.ContainsKey(address))
            {
                this.queues[address] = new BlockingQueue<MessagePayload>(blockingTimeout);
            }
        }
    }
}