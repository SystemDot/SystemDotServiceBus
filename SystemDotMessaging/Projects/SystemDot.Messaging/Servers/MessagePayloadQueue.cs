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
        readonly TimeSpan blockingTimeout;
        readonly Dictionary<Address, BlockingQueue<MessagePayload>> queues;
        
        public MessagePayloadQueue(TimeSpan blockingTimeout)
        {
            this.blockingTimeout = blockingTimeout;
            this.queues = new Dictionary<Address, BlockingQueue<MessagePayload>>();
        }

        public void Enqueue(MessagePayload toEnqueue)
        {
            Contract.Requires(toEnqueue != null);

            CreateQueueIfNonExistant(toEnqueue.Address);

            this.queues[toEnqueue.Address].Enqueue(toEnqueue);
        }

        public IEnumerable<MessagePayload> DequeueAll(Address address)
        {
            Contract.Requires(address != Address.Empty);
            
            CreateQueueIfNonExistant(address);
            
            return this.queues[address].DequeueAll();
        }

        void CreateQueueIfNonExistant(Address address)
        {
            if (!this.queues.ContainsKey(address))
            {
                this.queues[address] = new BlockingQueue<MessagePayload>(blockingTimeout);
            }
        }
    }
}